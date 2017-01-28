using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using TypeGen.Cli;
using TypeGen.Cli.Business;
using TypeGen.Cli.Extensions;
using TypeGen.Cli.Models;
using TypeGen.Core;

namespace TypeGen.Cli
{
    internal class Program
    {
        private static readonly ConsoleArgsReader _consoleArgsReader;
        private static readonly Logger _logger;
        private static readonly FileSystem _fileSystem;
        private static readonly ConfigProvider _configProvider;
        private static readonly GeneratorOptionsProvider _generatorOptionsProvider;
        private static readonly ProjectFileManager _projectFileManager;

        static Program()
        {
            _consoleArgsReader = new ConsoleArgsReader();
            _logger = new Logger();
            _fileSystem = new FileSystem();
            _configProvider = new ConfigProvider(_fileSystem, _logger, new JsonSerializer());
            _generatorOptionsProvider = new GeneratorOptionsProvider(_fileSystem, _logger);
            _projectFileManager = new ProjectFileManager(_fileSystem);
        }

        private static void Main(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    _logger.Log("Invalid usage. Please see help for more information (TypeGen -Help).");
                    return;
                }

                if (_consoleArgsReader.ContainsHelpParam(args))
                {
                    ShowHelp();
                    return;
                }

                if (_consoleArgsReader.ContainsGetCwdParam(args))
                {
                    string cwd = Directory.GetCurrentDirectory();
                    _logger.Log($"Current working directory is: {cwd}");
                    return;
                }

                bool verbose = _consoleArgsReader.ContainsVerboseParam(args);
                string[] projectFolders = _consoleArgsReader.GetProjectFolders(args).ToArray();
                string[] configPaths = _consoleArgsReader.GetConfigPaths(args).ToArray();

                for (var i = 0; i < projectFolders.Length; i++)
                {
                    string projectFolder = projectFolders[i];
                    string configPath = configPaths.HasIndex(i) ? configPaths[i] : null;

                    _logger.Log($"Generating files for project \"{projectFolder}\"...");
                    Generate(projectFolder, configPath, verbose);
                    _logger.Log($"Files for project \"{projectFolder}\" generated successfully.", "");
                }
            }
            catch (Exception e) when (e is CliException || e is CoreException)
            {
                _logger.Log($"APPLICATION ERROR: {e.Message}",
                    e.StackTrace);
            }
            catch (ReflectionTypeLoadException e)
            {
                foreach (Exception loaderException in e.LoaderExceptions)
                {
                    _logger.Log($"TYPE LOAD ERROR: {loaderException.Message}",
                        e.StackTrace);
                }
            }
            catch (Exception e)
            {
                _logger.Log($"GENERIC ERROR: {e.Message}",
                    e.StackTrace);
            }

            // DEBUG ONLY
            //Console.Read();
        }

        private static void Generate(string projectFolder, string configPath, bool verbose)
        {
            // get config

            configPath = !string.IsNullOrEmpty(configPath)
                ? $"{projectFolder}\\{configPath}"
                : $"{projectFolder}\\tgconfig.json";

            TgConfig config = _configProvider.GetConfig(configPath, projectFolder, verbose);
            IEnumerable<Assembly> assemblies = GetAssemblies(config.GetAssemblies());

            // create generator

            GeneratorOptions generatorOptions = _generatorOptionsProvider.GetGeneratorOptions(config, assemblies, projectFolder, verbose);
            generatorOptions.BaseOutputDirectory = projectFolder;
            var generator = new Generator { Options = generatorOptions };

            // generate

            IEnumerable<string> generatedFiles = assemblies
                .Aggregate(Enumerable.Empty<string>(), (acc, assembly) => acc.Concat(
                    generator.Generate(assembly).GeneratedFiles
                    ));

            if (config.AddFilesToProject ?? TgConfig.DefaultAddFilesToProject)
            {
                AddFilesToProject(projectFolder, generatedFiles);
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
            _logger.Log($"TypeGen v{AppConfig.Version}",
                "Usage: TypeGen ProjectFolder1[:ProjectFolder2:(...)] [-Config-Path \"path1[:path2:(...)]\"] [Get-Cwd] [-h | -Help] [-v | -Verbose]",
                "For more information please visit project's GitHub page: https://github.com/jburzynski/TypeGen");
        }
    }
}
