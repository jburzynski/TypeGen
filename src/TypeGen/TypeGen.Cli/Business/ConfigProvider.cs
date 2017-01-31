using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Cli.Models;
using TypeGen.Cli.Utils;

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
                if (logVerbose) _logger.Log($"No config file found for project \"{projectFolder}\". Default configuration will be used.");

                TgConfig defaultConfig = new TgConfig()
                    .MergeWithDefaultParams()
                    .Normalize();

                UpdateConfigAssemblyPaths(defaultConfig, projectFolder, logVerbose);
                return defaultConfig;
            }

            if (logVerbose) _logger.Log($"Reading the config file from \"{configPath}\"");

            TgConfig config = _jsonSerializer.DeserializeFromFile<TgConfig>(configPath)
                .MergeWithDefaultParams()
                .Normalize();

            UpdateConfigAssemblyPaths(config, projectFolder, logVerbose);

            return config;
        }

        private void UpdateConfigAssemblyPaths(TgConfig config, string projectFolder, bool logVerbose)
        {
            config.AssemblyPath = string.IsNullOrEmpty(config.AssemblyPath) ?
                null :
                GetAssemblyPath(config.AssemblyPath, projectFolder, logVerbose);

            config.Assemblies = config.Assemblies.Select(a => GetAssemblyPath(a, projectFolder, logVerbose)).ToArray();
        }

        private string GetAssemblyPath(string configAssemblyPath, string projectFolder, bool logVerbose)
        {
            if (string.IsNullOrEmpty(configAssemblyPath))
            {
                if (logVerbose) _logger.Log("Assembly path not found in the config file. Reading from the default assembly path (project folder's bin\\Debug or bin\\).");
                return GetDefaultAssemblyPath(projectFolder);
            }

            if (logVerbose) _logger.Log($"Reading assembly path from the config file: '{configAssemblyPath}'");
            string assemblyPath = $"{projectFolder}\\{configAssemblyPath}";

            if (!_fileSystem.FileExists(assemblyPath))
            {
                throw new CliException($"The specified assembly: '{configAssemblyPath}' not found in the project folder");
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
