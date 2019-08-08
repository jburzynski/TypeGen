using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using TypeGen.Cli.Business;
using TypeGen.Cli.Models;
using TypeGen.Core;
using TypeGen.Core.Extensions;
using TypeGen.Core.Generator;
using TypeGen.Core.Logging;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.Storage;
using IGeneratorOptionsProvider = TypeGen.Cli.Business.IGeneratorOptionsProvider;
using GeneratorOptionsProvider = TypeGen.Cli.Business.GeneratorOptionsProvider;

namespace TypeGen.Cli
{
    internal class Program
    {
        private static IConsoleArgsReader _consoleArgsReader;
        private static ILogger _logger;
        private static IFileSystem _fileSystem;
        private static IConfigProvider _configProvider;
        private static IGeneratorOptionsProvider _generatorOptionsProvider;
        private static IProjectFileManager _projectFileManager;
        private static ProjectBuilder _projectBuilder;
        private static IAssemblyResolver _assemblyResolver;

        private static void InitializeServices(string[] args)
        {
            _consoleArgsReader = new ConsoleArgsReader();

            bool verbose = _consoleArgsReader.ContainsVerboseOption(args);
            _logger = new ConsoleLogger(verbose);
            
            _fileSystem = new FileSystem();
            _configProvider = new ConfigProvider(_fileSystem, _logger);
            _generatorOptionsProvider = new GeneratorOptionsProvider(_fileSystem, _logger);
            _projectFileManager = new ProjectFileManager(_fileSystem);
            _projectBuilder = new ProjectBuilder(_logger);
        }

        private static void Main(string[] args)
        {
            try
            {
                InitializeServices(args);
                
                if (args == null || args.Length == 0 || _consoleArgsReader.ContainsHelpOption(args) || _consoleArgsReader.ContainsAnyCommand(args) == false)
                {
                    ShowHelp();
                    return;
                }

                if (_consoleArgsReader.ContainsGetCwdCommand(args))
                {
                    string cwd = _fileSystem.GetCurrentDirectory();
                    Console.WriteLine($"Current working directory is: {cwd}");
                    return;
                }
                
                string[] configPaths = _consoleArgsReader.GetConfigPaths(args).ToArray();

                string[] projectFolders = _consoleArgsReader.ContainsProjectFolderOption(args) ?
                    _consoleArgsReader.GetProjectFolders(args).ToArray() :
                    new [] { "." };

                for (var i = 0; i < projectFolders.Length; i++)
                {
                    string projectFolder = projectFolders[i];
                    string configPath = configPaths.HasIndex(i) ? configPaths[i] : null;

                    _assemblyResolver = new AssemblyResolver(_fileSystem, _logger, projectFolder);

                    Generate(projectFolder, configPath);
                }
            }
            catch (Exception e) when (e is CliException || e is CoreException)
            {
                _logger.Log($"APPLICATION ERROR: {e.Message}{Environment.NewLine}{e.StackTrace}", LogLevel.Error);
            }
            catch (AssemblyResolutionException e)
            {
                string message = e.Message +
                                 "Consider adding any external assembly directories in the externalAssemblyPaths parameter. " +
                                 "If you're using ASP.NET Core, add your NuGet directory to externalAssemblyPaths parameter (you can use global NuGet packages directory alias: \"<global-packages>\")";
                _logger.Log($"{message}{Environment.NewLine}{e.StackTrace}", LogLevel.Error);
            }
            catch (ReflectionTypeLoadException e)
            {
                foreach (Exception loaderException in e.LoaderExceptions)
                {
                    _logger.Log($"TYPE LOAD ERROR: {loaderException.Message}{Environment.NewLine}{e.StackTrace}", LogLevel.Error);
                }
            }
            catch (Exception e)
            {
                _logger.Log($"GENERIC ERROR: {e.Message}{Environment.NewLine}{e.StackTrace}", LogLevel.Error);
            }
        }

        private static void Generate(string projectFolder, string configPath)
        {
            // get config

            configPath = !string.IsNullOrEmpty(configPath)
                ? Path.Combine(projectFolder, configPath)
                : Path.Combine(projectFolder, "tgconfig.json");

            TgConfig config = _configProvider.GetConfig(configPath, projectFolder);
            LogConfigWarnings(config);

            // register assembly resolver

            _assemblyResolver.Directories = config.ExternalAssemblyPaths;
            _assemblyResolver.Register();

            IEnumerable<Assembly> assemblies = GetAssemblies(config.GetAssemblies()).ToArray();

            // create generator

            GeneratorOptions generatorOptions = _generatorOptionsProvider.GetGeneratorOptions(config, assemblies, projectFolder);
            generatorOptions.BaseOutputDirectory = Path.Combine(projectFolder, config.OutputPath);
            var generator = new Generator(generatorOptions, _logger);

            // generate
            
            if (config.ClearOutputDirectory == true) _fileSystem.ClearDirectory(generatorOptions.BaseOutputDirectory);
            if (config.BuildProject == true) _projectBuilder.Build(projectFolder);
            
            _logger.Log($"Generating files for project \"{projectFolder}\"...", LogLevel.Info);
            
            IEnumerable<string> generatedFiles = Enumerable.Empty<string>();

            if (!config.GenerationSpecs.Any() || config.GenerateFromAssemblies == true)
            {
                generatedFiles = generator.Generate(assemblies);
            }

            if (config.GenerationSpecs.Any())
            {
                var typeResolver = new TypeResolver(_logger, _fileSystem, projectFolder, assemblies);
                
                IEnumerable<GenerationSpec> generationSpecs = config.GenerationSpecs
                    .Select(name => typeResolver.Resolve(name, "GenerationSpec"))
                    .Where(t => t != null)
                    .Select(t => (GenerationSpec)Activator.CreateInstance(t))
                    .ToArray();
                    
                generatedFiles = generatedFiles.Concat(generator.Generate(generationSpecs));
            }

            generatedFiles = generatedFiles.ToArray();
            
            foreach (string file in generatedFiles)
            {
                _logger.Log($"Generated {file}", LogLevel.Info);
            }
            
            if (config.AddFilesToProject ?? TgConfig.DefaultAddFilesToProject)
            {
                AddFilesToProject(projectFolder, generatedFiles);
            }

            // unregister assembly resolver

            _assemblyResolver.Unregister();
            
            _logger.Log($"Files for project \"{projectFolder}\" generated successfully.{Environment.NewLine}", LogLevel.Info);
        }

        private static void LogConfigWarnings(TgConfig config)
        {
            if (config.CreateIndexFile == true)
            {
                _logger.Log("Deprecated 'createIndexFile' CLI option used. This option may be removed in future versions.", LogLevel.Warning);
            }
        }

        private static void AddFilesToProject(string projectFolder, IEnumerable<string> generatedFiles)
        {
            XmlDocument projectFile = _projectFileManager.ReadFromProjectFolder(projectFolder);

            foreach (string filePath in generatedFiles)
            {
                _projectFileManager.AddTsFile(projectFile, filePath);
            }

            _projectFileManager.SaveProjectFile(projectFolder, projectFile);
        }

        private static IEnumerable<Assembly> GetAssemblies(IEnumerable<string> assemblyNames)
        {
            return assemblyNames.Select(Assembly.LoadFrom);
        }

        private static void ShowHelp()
        {
            Console.WriteLine($"TypeGen v{AppConfig.Version}",
                "",
                "Usage: [dotnet] typegen [options] [command]",
                "",
                "Options:",
                "-h|--help               Show help information",
                "-v|--verbose            Show verbose output",
                "-p|--project-folder     Set project folder path(s)",
                "-c|--config-path        Set config path(s) to use",
                "",
                "Commands:",
                "generate     Generate TypeScript files",
                "getcwd       Get current working directory",
                "",
                "For more information please visit project's website: http://jburzynski.net/TypeGen");
        }
    }
}
