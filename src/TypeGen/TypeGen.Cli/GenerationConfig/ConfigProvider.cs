using Newtonsoft.Json;
using NuGet.Frameworks;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TypeGen.Cli.Extensions;
using TypeGen.Cli.TypeResolution;
using TypeGen.Core;
using TypeGen.Core.Logging;
using TypeGen.Core.Storage;
using TypeGen.Core.Validation;

namespace TypeGen.Cli.GenerationConfig
{
    internal class ConfigProvider : IConfigProvider
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger _logger;

        public ConfigProvider(IFileSystem fileSystem,
            ILogger logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        /// <summary>
        /// Creates an instance of TgConfig from 
        /// </summary>
        /// <param name="configPath"></param>
        /// <param name="projectFolder"></param>
        /// <param name="consoleOptions"></param>
        /// <returns></returns>
        public TgConfig GetConfig(string configPath, string projectFolder, ConfigConsoleOptions consoleOptions)
        {
            Requires.NotNullOrEmpty(projectFolder, nameof(projectFolder));
            
            var isConfigPathProvided = !string.IsNullOrEmpty(configPath);

            configPath = !string.IsNullOrEmpty(configPath)
                ? configPath.RelativeOrRooted(projectFolder)
                : "tgconfig.json".RelativeOrRooted(projectFolder);

            if (isConfigPathProvided && !_fileSystem.FileExists(configPath))
                throw new CliException($"The config file path does not exist: '{configPath}'.");

            _logger.Log(_fileSystem.FileExists(configPath)
                ? $"Reading the config file from \"{configPath}\""
                : $"No config file found for project \"{projectFolder}\". Default configuration will be used.",
                LogLevel.Debug);

            var config = _fileSystem.FileExists(configPath)
                ? JsonConvert.DeserializeObject<TgConfig>(_fileSystem.ReadFile(configPath))
                : new TgConfig();

            OverrideWithConsoleOptions(config, consoleOptions);
            config.MergeWithDefaultParams();
            config.Normalize();
            
            UpdateConfigAssemblyPaths(config, projectFolder);
            
            return config;
        }

        private static void OverrideWithConsoleOptions(TgConfig config, ConfigConsoleOptions consoleOptions)
        {
            if (consoleOptions.OutputFolder != null) config.OutputPath = consoleOptions.OutputFolder;
        }

        private void UpdateConfigAssemblyPaths(TgConfig config, string projectFolder)
        {
            if (!string.IsNullOrEmpty(config.AssemblyPath))
            {
                config.AssemblyPath = GetAssemblyPathRelativeToCwd(config.AssemblyPath, projectFolder, config.ProjectOutputFolder);
                _logger.Log("The 'assemblyPath' config parameter is deprecated and can be removed in future versions. Please use 'assemblies' instead.", LogLevel.Warning);
            }

            config.Assemblies = config.Assemblies.Select(a => GetAssemblyPathRelativeToCwd(a, projectFolder, config.ProjectOutputFolder)).ToArray();

            if (!config.Assemblies.Any())
            {
                config.Assemblies = new[] { GetAssemblyPathRelativeToCwd(null, projectFolder, config.ProjectOutputFolder) };
            }
        }

        private string GetAssemblyPathRelativeToCwd(string assemblyPathUserInput, string projectFolder, string projectOutputFolder)
        {
            if (string.IsNullOrEmpty(assemblyPathUserInput))
            {
                _logger.Log($"Assembly path not found in the config file. Assembly file will be searched for recursively in the project's output folder '{projectOutputFolder}\\'.", LogLevel.Debug);
                return GetDefaultAssemblyPath(projectFolder, projectOutputFolder);
            }

            _logger.Log($"Reading the assembly path from the config file: '{assemblyPathUserInput}'", LogLevel.Debug);
            string assemblyPath = Path.Combine(projectFolder, assemblyPathUserInput);

            if (!_fileSystem.FileExists(assemblyPath))
            {
                throw new CliException($"The specified assembly: '{assemblyPathUserInput}' not found in the project folder");
            }

            return assemblyPath;
        }

        private string GetDefaultAssemblyPath(string projectFolder, string projectOutputFolder)
        {
            string projectFileName = _fileSystem.GetDirectoryFiles(projectFolder)
                .Select(Path.GetFileName)
                .FirstOrDefault(n => n.EndsWith(".csproj") || n.EndsWith(".xproj"));

            if (projectFileName == null)
                throw new CliException("Project file (*.csproj or *.xproj) not found in the project folder and no assembly path found in the config file");

            string dllFileName = Path.ChangeExtension(projectFileName, "dll");
            string exeFileName = Path.ChangeExtension(projectFileName, "exe");

            string projectFilePath = Path.Combine(projectFolder, projectFileName);
            string projectOutputFolderRelativeToCwd = Path.Combine(projectFolder, projectOutputFolder);

            if (!_fileSystem.DirectoryExists(projectOutputFolderRelativeToCwd))
                throw new CliException($"The project's output folder '{projectOutputFolderRelativeToCwd}' does not exist. Make sure the project is built.");

            List<string> foundFiles = _fileSystem.GetFilesRecursive(projectOutputFolderRelativeToCwd, dllFileName)
                .Concat(_fileSystem.GetFilesRecursive(projectOutputFolderRelativeToCwd, exeFileName))
                .ToList();

            string foundFile = null;

            if (foundFiles.Count > 0)
            {
                var projectTargetFrameworks = TargetFrameworkUtils.GetProjectTargetFrameworks(projectFilePath);

                foundFile = foundFiles[0];

                if (foundFiles.Count > 1)
                {
                    foundFile = foundFiles
                        .Select(x => (path: x, assemblyFramework: TargetFrameworkUtils.GetAssemblyTargetFramework(x)))
                        .Where(x => x.assemblyFramework != null && !x.assemblyFramework.IsUnsupported &&
                            projectTargetFrameworks.Any(f => f == x.assemblyFramework) &&
                            DefaultCompatibilityProvider.Instance.IsCompatible(RuntimeInfo.CliFramework, x.assemblyFramework))
                        .OrderByDescending(x => _fileSystem.FileExists(Path.ChangeExtension(x.path, "xml")))
                        .ThenBy(x => TargetFrameworkUtils.GetFrameworkFamilyRank(x.assemblyFramework!))
                        .ThenByDescending(x => x.assemblyFramework!.Version)
                        .ThenByDescending(x => _fileSystem.GetLastWriteTimeUtc(x.path))
                        .Select(x => x.path)
                        .FirstOrDefault();
                }
            }

            if (foundFile != null)
            {
                _logger.Log($"Using project assembly found in: {foundFile}", LogLevel.Debug);
                return foundFile;
            }

            throw new CliException($"Could not find a compatible assembly in the project's output folder '{projectOutputFolderRelativeToCwd}' (searched recursively). Make sure the project is built and targets a .NET version compatible with the CLI ({RuntimeInfo.CliFramework}).");
        }
    }
}
