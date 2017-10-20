using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Cli.Models;
using TypeGen.Core.Utils;
using TypeGen.Core.Storage;

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
            if (!string.IsNullOrEmpty(config.AssemblyPath))
            {
                config.AssemblyPath = GetAssemblyPath(config.AssemblyPath, projectFolder, logVerbose);
                _logger.Log("WARNING: assemblyPath config parameter is deprecated and can be removed in future versions. Please use 'assemblies' instead.");
            }

            config.Assemblies = config.Assemblies.Select(a => GetAssemblyPath(a, projectFolder, logVerbose)).ToArray();

            if (!config.Assemblies.Any())
            {
                config.Assemblies = new[] { GetAssemblyPath(null, projectFolder, logVerbose) };
            }
        }

        private string GetAssemblyPath(string configAssemblyPath, string projectFolder, bool logVerbose)
        {
            if (string.IsNullOrEmpty(configAssemblyPath))
            {
                if (logVerbose) _logger.Log("Assembly path not found in the config file. Assembly file will be searched for recursively in the project's bin\\.");
                return GetDefaultAssemblyPath(projectFolder);
            }

            if (logVerbose) _logger.Log($"Reading assembly path from the config file: '{configAssemblyPath}'");
            string assemblyPath = $"{projectFolder}{Path.DirectorySeparatorChar}{configAssemblyPath}";

            if (!_fileSystem.FileExists(assemblyPath))
            {
                throw new CliException($"The specified assembly: '{configAssemblyPath}' not found in the project folder");
            }

            return assemblyPath;
        }

        private string GetDefaultAssemblyPath(string projectFolder)
        {
            string projectFileName = _fileSystem.GetDirectoryFiles(projectFolder)
                .Select(FileSystemUtils.GetFileNameFromPath)
                .FirstOrDefault(n => n.EndsWith(".csproj") || n.EndsWith(".xproj"));

            if (projectFileName == null)
            {
                throw new CliException("Project file (*.csproj or *.xproj) not found in the project folder and no assembly path found in the config file");
            }

            string dllFileName = Path.ChangeExtension(projectFileName, "dll");
            string exeFileName = Path.ChangeExtension(projectFileName, "exe");
            string binPath = $"{projectFolder}{Path.DirectorySeparatorChar}bin";

            IEnumerable<string> foundFiles = _fileSystem.GetFilesRecursive(binPath, dllFileName)
                .Concat(_fileSystem.GetFilesRecursive(binPath, exeFileName));

            if (foundFiles.Any()) return foundFiles.First();

            throw new CliException($"None of: '{dllFileName}' or '{exeFileName}' found in the default assembly folder (the project's bin\\ folder; searched recursively). Please make sure you have your project built.");
        }
    }
}
