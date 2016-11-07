using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TypeGen.Cli.Business;
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

        static Program()
        {
            _consoleArgsReader = new ConsoleArgsReader();
            _logger = new Logger();
            _fileSystem = new FileSystem();
            _configProvider = new ConfigProvider(_fileSystem, _logger, new JsonSerializer());
            _generatorOptionsProvider = new GeneratorOptionsProvider(_fileSystem, _logger);
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

                string projectFolder = _consoleArgsReader.GetProjectFolder(args);
                if (!_fileSystem.DirectoryExists(projectFolder))
                {
                    throw new CliException($"Project folder '{projectFolder}' does not exist");
                }

                // get config

                string configPath = _consoleArgsReader.GetConfigPath(args);
                configPath = !string.IsNullOrEmpty(configPath)
                    ? $"{projectFolder}\\{configPath}"
                    : $"{projectFolder}\\tgconfig.json";

                TgConfig config = _configProvider.GetConfig(configPath, projectFolder, verbose);

                // get assembly

                Assembly assembly = Assembly.LoadFrom(config.AssemblyPath);

                // create generator options

                GeneratorOptions generatorOptions = _generatorOptionsProvider.GetGeneratorOptions(config, assembly, projectFolder, verbose);
                generatorOptions.BaseOutputDirectory = projectFolder;

                var generator = new Generator {Options = generatorOptions};
                generator.Generate(assembly);

                _logger.Log("Files generated successfully. Exiting...");
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
        }

        private static void ShowHelp()
        {
            _logger.Log($"TypeGen {AppConfig.Version}",
                "Usage: TypeGen ProjectFolder [-Config-Path \"config\\path.json\"] [Get-Cwd] [-h | -Help] [-v | -Verbose]",
                "For more information please visit project's GitHub page: https://github.com/jburzynski/TypeGen");
        }
    }
}
