using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TypeGen.Core;

namespace TypeGen.Cli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Contains("-Load-Tg-Test"))
            {
                GenerateFromTest();
                return;
            }

            bool verbose = args.Any(arg => arg.ToUpperInvariant() == "-V" || arg.ToUpperInvariant() == "-VERBOSE");

            string projectFolder = args[0];

            if (projectFolder.Contains('\\') || projectFolder.Contains('/'))
            {
                throw new CliException("Project folder name should not contain slashes ('/' or '\\')");
            }

            string configPath = GetConfigPath(args);
            configPath = !string.IsNullOrEmpty(configPath)
                ? $"{projectFolder}\\{configPath}"
                : $"{projectFolder}\\tgconfig.json";

            string configJson;

            if (File.Exists(configPath))
            {
                if (verbose) Console.WriteLine($"Reading config from \"{configPath}\"");
                configJson = File.ReadAllText(configPath);
            }
            else
            {
                if (verbose) Console.WriteLine("No config file found. Default configuration will be used.");
                configJson = Utilities.GetEmbeddedResource("TypeGen.Cli.defaultConfig.json");
            }

            string csProjFileName = Directory.GetFiles(projectFolder)
                .Select(Utilities.GetFileNameFromPath)
                .FirstOrDefault(n => n.EndsWith(".csproj"));

            string debugFolder = projectFolder + "\\bin\\Debug\\";

            string assemblyFileName = Utilities.ChangeFileExtension(csProjFileName, "dll");
            if (!File.Exists(debugFolder + assemblyFileName))
            {
                assemblyFileName = Utilities.ChangeFileExtension(csProjFileName, "exe");
            }

            Assembly assembly = csProjFileName != null ? Assembly.LoadFrom(debugFolder + assemblyFileName) : null;

            var generator = new Generator(projectFolder);
            generator.Generate(assembly);
        }

        private static string GetConfigPath(string[] args)
        {
            List<string> argsList = args.ToList();
            int index = argsList.IndexOf("-ConfigPath");

            if (index < 0) return null;

            if (args.Length < index + 2) // index of the next element + 1
            {
                throw new CliException("-Config-Path parameter present, but no path specified");
            }

            return args[index + 1].NormalizePath();
        }

        private static void GenerateFromTest()
        {
            Assembly assembly = Assembly.LoadFrom("TypeGen.Test.dll");
            var generator = new Generator();
            generator.Generate(assembly);
        }
    }
}
