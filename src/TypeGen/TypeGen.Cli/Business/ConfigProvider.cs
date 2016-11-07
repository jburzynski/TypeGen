using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Cli.Models;
using TypeGen.Core.Utils;

namespace TypeGen.Cli.Business
{
    internal class ConfigProvider
    {
        private readonly FileSystem _fileSystem;
        private readonly Logger _logger;
        private readonly JsonSerializer _jsonSerializer;

        public ConfigProvider(FileSystem fileSystem,
            Logger logger,
            JsonSerializer jsonSerializer)
        {
            _fileSystem = fileSystem;
            _logger = logger;
            _jsonSerializer = jsonSerializer;
        }

        /// <summary>
        /// Creates a config object from a given config file
        /// </summary>
        /// <param name="configPath"></param>
        /// <param name="projectFolder"></param>
        /// <param name="logVerbose"></param>
        /// <returns></returns>
        public TgConfig GetConfig(string configPath, string projectFolder, bool logVerbose)
        {
            if (!_fileSystem.FileExists(configPath))
            {
                if (logVerbose) _logger.Log("No config file found. Default configuration will be used.");

                var defaultConfig = new TgConfig();
                defaultConfig.AssemblyPath = GetAssemblyPath(defaultConfig, projectFolder, logVerbose);
                return defaultConfig;
            }

            if (logVerbose) _logger.Log($"Reading the config file from \"{configPath}\"");

            TgConfig config = _jsonSerializer.DeserializeFromFile<TgConfig>(configPath)
                .MergeWithDefaultParams()
                .Normalize();

            config.AssemblyPath = GetAssemblyPath(config, projectFolder, logVerbose);

            return config;
        }

        private string GetAssemblyPath(TgConfig config, string projectFolder, bool logVerbose)
        {
            if (string.IsNullOrEmpty(config.AssemblyPath))
            {
                if (logVerbose) _logger.Log("Assembly path not found in the config file. Reading from the default assembly path (project folder's bin\\Debug or bin\\).");
                return GetDefaultAssemblyPath(projectFolder);
            }

            if (logVerbose) _logger.Log($"Reading assembly path from the config file: '{config.AssemblyPath}'");
            string assemblyPath = $"{projectFolder}\\{config.AssemblyPath}";

            if (!_fileSystem.FileExists(assemblyPath))
            {
                throw new CliException($"The specified assembly: '{config.AssemblyPath}' not found in the project folder");
            }

            return assemblyPath;
        }

        private string GetDefaultAssemblyPath(string projectFolder)
        {
            string csProjFileName = _fileSystem.GetDirectoryFiles(projectFolder)
                .Select(FileSystemUtils.GetFileNameFromPath)
                .FirstOrDefault(n => n.EndsWith(".csproj"));

            if (csProjFileName == null)
            {
                throw new CliException("Project file (*.csproj) not found in the project folder and no assembly path found in the config file");
            }

            string binDebugPath = $"{projectFolder}\\bin\\Debug\\";
            string binPath = $"{projectFolder}\\bin\\";

            string assemblyFileName = Path.ChangeExtension(csProjFileName, "dll");

            if (_fileSystem.FileExists(binDebugPath + assemblyFileName)) return binDebugPath + assemblyFileName;
            if (_fileSystem.FileExists(binPath + assemblyFileName)) return binPath + assemblyFileName;

            string assemblyFileNameExe = Path.ChangeExtension(csProjFileName, "exe");

            if (_fileSystem.FileExists(binDebugPath + assemblyFileNameExe)) return binDebugPath + assemblyFileNameExe;
            if (_fileSystem.FileExists(binPath + assemblyFileNameExe)) return binPath + assemblyFileNameExe;

            throw new CliException($"None of: '{assemblyFileName}' or '{assemblyFileNameExe}' found in the default assembly folder (the project's bin\\Debug or bin\\ folders). Please make sure you have your project built.");
        }
    }
}
