using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using TypeGen.Cli.Build;
using TypeGen.Cli.Business;
using TypeGen.Cli.ProjectFileManagement;
using TypeGen.Cli.TypeGenConfig;
using TypeGen.Cli.TypeResolution;
using TypeGen.Core;
using TypeGen.Core.Extensions;
using TypeGen.Core.Generator;
using TypeGen.Core.Logging;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.Storage;
using IGeneratorOptionsProvider = TypeGen.Cli.TypeGenConfig.IGeneratorOptionsProvider;
using GeneratorOptionsProvider = TypeGen.Cli.TypeGenConfig.GeneratorOptionsProvider;

namespace TypeGen.Cli
{
    internal class Program
    {
        private static ILogger _logger;
        private static IFileSystem _fileSystem;
        private static IConfigProvider _configProvider;
        private static IGeneratorOptionsProvider _generatorOptionsProvider;
        private static IProjectFileManager _projectFileManager;
        private static ProjectBuild _projectBuild;
        private static IAssemblyResolver _assemblyResolver;

        private static void InitializeServices(string[] args)
        {
            bool verbose = ConsoleArgsReader.ContainsVerboseOption(args);
            _logger = new ConsoleLogger(verbose);
            
            _fileSystem = new FileSystem();
            _configProvider = new ConfigProvider(_fileSystem, _logger);
            _projectFileManager = new ProjectFileManager(_fileSystem);
            _projectBuild = new ProjectBuild(_logger);
        }

        private static int Main(string[] args)
        {
            try
            {
                InitializeServices(args);
                
                if (args == null || args.Length == 0 || ConsoleArgsReader.ContainsHelpOption(args) || ConsoleArgsReader.ContainsAnyCommand(args) == false)
                {
                    ShowHelp();
                    return (int)ExitCode.Success;
                }

                if (ConsoleArgsReader.ContainsGetCwdCommand(args))
                {
                    string cwd = _fileSystem.GetCurrentDirectory();
                    Console.WriteLine($"Current working directory is: {cwd}");
                    return (int)ExitCode.Success;
                }
                
                string[] configPaths = ConsoleArgsReader.GetConfigPaths(args).ToArray();

                string[] projectFolders = ConsoleArgsReader.ContainsProjectFolderOption(args) ?
                    ConsoleArgsReader.GetProjectFolders(args).ToArray() :
                    new [] { "." };

                string? outputFolder = ConsoleArgsReader.ContainsOutputOption(args) ?
                    ConsoleArgsReader.GetOutputFolder(args) : null;

                for (var i = 0; i < projectFolders.Length; i++)
                {
                    string projectFolder = projectFolders[i];
                    string configPath = configPaths.HasIndex(i) ? configPaths[i] : null;

                    _assemblyResolver = new AssemblyResolver(_fileSystem, _logger, projectFolder);

                    Generate(projectFolder, configPath, outputFolder);
                }
                
                return (int)ExitCode.Success;
            }
            catch (AssemblyResolutionException e)
            {
                string message = e.Message +
                                 "Consider adding any external assembly directories in the externalAssemblyPaths parameter. " +
                                 "If you're using ASP.NET Core, add your NuGet directory to externalAssemblyPaths parameter (you can use global NuGet packages directory alias: \"<global-packages>\")";
                _logger.Log($"{message}{Environment.NewLine}{e.StackTrace}", LogLevel.Error);
                return (int)ExitCode.Error;
            }
            catch (ReflectionTypeLoadException e)
            {
                foreach (Exception loaderException in e.LoaderExceptions)
                {
                    _logger.Log($"TYPE LOAD ERROR: {loaderException.Message}{Environment.NewLine}{e.StackTrace}", LogLevel.Error);
                }
                return (int)ExitCode.Error;
            }
            catch (Exception e)
            {
                _logger.Log($"ERROR: {e.Message}{Environment.NewLine}{e.StackTrace}", LogLevel.Error);
                return (int)ExitCode.Error;
            }
        }

        private static void Generate(string projectFolder, string configPath, string? outputFolder)
        {
            // get config

            configPath = !string.IsNullOrEmpty(configPath)
                ? Path.Combine(projectFolder, configPath)
                : Path.Combine(projectFolder, "tgconfig.json");

            TgConfig config = _configProvider.GetConfig(configPath, projectFolder);

            // register assembly resolver

            _assemblyResolver.Directories = config.ExternalAssemblyPaths;
            _assemblyResolver.Register();

            IEnumerable<Assembly> assemblies = GetAssemblies(config.GetAssemblies()).ToArray();

            // create generator

            var typeResolver = new TypeResolver(_logger, _fileSystem, projectFolder, assemblies);
            _generatorOptionsProvider = new GeneratorOptionsProvider(typeResolver);
            GeneratorOptions generatorOptions = _generatorOptionsProvider.GetGeneratorOptions(config, assemblies, projectFolder);
            generatorOptions.BaseOutputDirectory = outputFolder ?? Path.Combine(projectFolder, config.OutputPath);
            var generator = new Generator(generatorOptions, _logger);

            // generate
            
            if (config.ClearOutputDirectory == true) _fileSystem.ClearDirectory(generatorOptions.BaseOutputDirectory);
            if (config.BuildProject == true) _projectBuild.Build(projectFolder);
            
            _logger.Log($"Generating files for project \"{projectFolder}\"...", LogLevel.Info);
            
            var generatedFiles = new List<string>();

            if (!config.GenerationSpecs.Any() || config.GenerateFromAssemblies == true)
            {
                generatedFiles.AddRange(generator.Generate(assemblies));
            }

            if (config.GenerationSpecs.Any())
            {
                IEnumerable<GenerationSpec> generationSpecs = config.GenerationSpecs
                    .Select(name => typeResolver.Resolve(name, "GenerationSpec"))
                    .Where(t => t != null)
                    .Select(t => (GenerationSpec)Activator.CreateInstance(t))
                    .ToArray();
                    
                generatedFiles.AddRange(generator.Generate(generationSpecs));
            }

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
            Console.WriteLine($"TypeGen v{ApplicationConfig.Version}" + Environment.NewLine +
                              Environment.NewLine +
                              "Usage: [dotnet-]typegen [options] [command]" + Environment.NewLine +
                              Environment.NewLine +
                              "Options:" + Environment.NewLine +
                              "-h|--help               Show help information" + Environment.NewLine +
                              "-v|--verbose            Show verbose output" + Environment.NewLine +
                              "-p|--project-folder     Set project folder path(s)" + Environment.NewLine +
                              "-c|--config-path        Set config path(s) to use" + Environment.NewLine +
                              Environment.NewLine +
                              "Commands:" + Environment.NewLine +
                              "generate     Generate TypeScript files" + Environment.NewLine +
                              "getcwd       Get current working directory" + Environment.NewLine +
                              Environment.NewLine +
                              "For more information, visit:" + Environment.NewLine +
                              "Documentation: https://typegen.readthedocs.io" + Environment.NewLine +
                              "GitHub: https://github.com/jburzynski/TypeGen");
        }
    }
}
