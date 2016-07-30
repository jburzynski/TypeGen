using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using TypeGen.Core;
using TypeGen.Core.Converters;

namespace TypeGen.Cli
{
    internal class Program
    {
        private static bool verbose;
        private static string projectFolder;
        private static Assembly assembly; // the loaded assembly for TS generation

        private static void Main(string[] args)
        {
            try
            {
                if (args == null)
                {
                    Console.WriteLine("Aruments array is null. Exiting...");
                    return;
                }

                if (args.Any(arg => arg.ToUpperInvariant() == "-H" || arg.ToUpperInvariant() == "-HELP"))
                {
                    ShowHelp();
                    return;
                }

                if (args.Any(arg => arg.ToUpperInvariant() == "GET-CWD"))
                {
                    string cwd = Directory.GetCurrentDirectory();
                    Console.WriteLine($"Current working directory is: {cwd}");
                    return;
                }

                verbose = args.Any(arg => arg.ToUpperInvariant() == "-V" || arg.ToUpperInvariant() == "-VERBOSE");

                if (args.Length == 0)
                {
                    Console.WriteLine("Invalid usage. Please see help for more information (TypeGen -Help).");
                    return;
                }

                projectFolder = args[0].NormalizePath();
                if (!Directory.Exists(projectFolder))
                {
                    throw new CliException($"Project folder '{projectFolder}' does not exist");
                }

                // get config

                string configPath = GetConfigPath(args);
                configPath = !string.IsNullOrEmpty(configPath)
                    ? $"{projectFolder}\\{configPath}"
                    : $"{projectFolder}\\tgconfig.json";

                ConfigParams configParams = CreateConfigParams(configPath)
                    .MergeWithDefaultParams()
                    .Normalize();

                // get assembly

                string assemblyPath = GetAssemblyPath(configParams);
                assembly = Assembly.LoadFrom(assemblyPath);

                // create generator options

                GeneratorOptions generatorOptions = GetGeneratorOptions(configParams);
                generatorOptions.BaseOutputDirectory = projectFolder;

                var generator = new Generator {Options = generatorOptions};
                generator.Generate(assembly);

                Console.WriteLine("Files generated successfully. Exiting...");
            }
            catch (Exception e) when (e is CliException || e is CoreException)
            {
                Console.WriteLine($"APPLICATION ERROR: {e.Message}");
                Console.WriteLine("Exiting...");
            }
            catch (ReflectionTypeLoadException e)
            {
                foreach (Exception loaderException in e.LoaderExceptions)
                {
                    Console.WriteLine($"TYPE LOAD ERROR: {loaderException.Message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"GENERIC ERROR: {e.Message}");
                Console.WriteLine("Exiting...");
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Usage: TypeGen ProjectFolder [-Config-Path \"config\\path.json\"] [Get-Cwd] [-h | -Help] [-v | -Verbose]");
            Console.WriteLine("For more information please visit project's GitHub page: https://github.com/jburzynski/TypeGen");
        }

        private static GeneratorOptions GetGeneratorOptions(ConfigParams configParams)
        {
            return new GeneratorOptions
            {
                TypeScriptFileExtension = configParams.TypeScriptFileExtension,
                TabLength = configParams.TabLength.Value,
                ExplicitPublicAccessor = configParams.ExplicitPublicAccessor.Value,
                FileNameConverters = GetTypeNameConvertersFromConfig(configParams.FileNameConverters),
                TypeNameConverters = GetTypeNameConvertersFromConfig(configParams.TypeNameConverters),
                PropertyNameConverters = GetNameConvertersFromConfig(configParams.PropertyNameConverters),
                EnumValueNameConverters = GetNameConvertersFromConfig(configParams.EnumValueNameConverters)
            };

        }

        private static TypeNameConverterCollection GetTypeNameConvertersFromConfig(string[] typeNameConverters)
        {
            IEnumerable<ITypeNameConverter> converters = typeNameConverters.Select(GetConverterFromName<ITypeNameConverter>);
            return new TypeNameConverterCollection(converters);
        }

        private static NameConverterCollection GetNameConvertersFromConfig(string[] nameConverters)
        {
            IEnumerable<INameConverter> converters = nameConverters.Select(GetConverterFromName<INameConverter>);
            return new NameConverterCollection(converters);
        }

        private static TConverter GetConverterFromName<TConverter>(string name) where TConverter: class, IConverter
        {
            string[] nameParts = name.Split(':');

            if (nameParts.Length == 1)
            {
                return GetConverterNoAssembly<TConverter>(name);
            }

            if (nameParts.Length == 2)
            {
                return GetConverterFromAssembly<TConverter>(nameParts[0], nameParts[1]);
            }

            throw new CliException($"Failed to load converter '{name}'. Incorrect name format.");
        }

        private static TConverter GetConverterFromAssembly<TConverter>(string assemblyPath, string name) where TConverter: class, IConverter
        {
            string assemblyFullPath = $"{projectFolder}\\{assemblyPath}";
            if (!File.Exists(assemblyFullPath))
            {
                // should never happen
                throw new CliException($"Assembly path '{assemblyFullPath}' not found for converter '{name}'");
            }

            Assembly converterAssembly = Assembly.LoadFrom(assemblyFullPath);
            return GetConverterFromAssembly<TConverter>(converterAssembly, name);
        }

        private static TConverter GetConverterFromAssembly<TConverter>(Assembly converterAssembly, string converterName) where TConverter : class, IConverter
        {
            foreach (Type type in converterAssembly.GetTypes())
            {
                bool nameMatches = (type.Name == converterName
                                   || type.Name == $"{converterName}Converter"
                                   || type.FullName == converterName
                                   || type.FullName == $"{converterName}Converter");
                bool typeMatches = type.GetInterfaces().Any(i => i == typeof(TConverter));

                if (nameMatches && typeMatches)
                {
                    return (TConverter)Activator.CreateInstance(type);
                }
            }

            return null;
        }

        private static TConverter GetConverterNoAssembly<TConverter>(string name) where TConverter : class, IConverter
        {
            // first, try to get the converter from the current loaded assembly
            var result = GetConverterFromAssembly<TConverter>(assembly, name);
            if (result != null)
            {
                if (verbose) Console.WriteLine($"Converter '{name}' found in assembly '{assembly.FullName}'");
                return result;
            }

            if (verbose) Console.WriteLine($"Converter '{name}' not found in assembly '{assembly.FullName}'. Falling back to TypeGen.Core.");

            // fallback to TypeGen.Core
            Assembly coreAssembly = typeof(Generator).Assembly;
            result = GetConverterFromAssembly<TConverter>(coreAssembly, name);
            if (result != null)
            {
                if (verbose) Console.WriteLine($"Converter '{name}' found in TypeGen.Core");
                return result;
            }

            throw new CliException($"Converter '{name}' not found in assembly '{assembly.FullName}' or TypeGen.Core");
        }

        private static string GetAssemblyPath(ConfigParams configParams)
        {
            if (string.IsNullOrEmpty(configParams.AssemblyPath))
            {
                if (verbose) Console.WriteLine("Assembly path not found in the config file. Reading from the default assembly path (project folder's bin\\Debug or bin\\).");
                return GetDefaultAssemblyPath();
            }

            if (verbose) Console.WriteLine($"Reading assembly path from the config file: '{configParams.AssemblyPath}'");
            string assemblyPath = $"{projectFolder}\\{configParams.AssemblyPath}";

            if (!File.Exists(assemblyPath))
            {
                throw new CliException($"The specified assembly: '{configParams.AssemblyPath}' not found in the project folder");
            }

            return assemblyPath;
        }

        private static string GetDefaultAssemblyPath()
        {
            string csProjFileName = Directory.GetFiles(projectFolder)
                .Select(Utilities.GetFileNameFromPath)
                .FirstOrDefault(n => n.EndsWith(".csproj"));

            if (csProjFileName == null)
            {
                throw new CliException("Project file (*.csproj) not found in the project folder and no assembly path found in the config file");
            }

            string binDebugPath = $"{projectFolder}\\bin\\Debug\\";
            string binPath = $"{projectFolder}\\bin\\";

            string assemblyFileName = Utilities.ChangeFileExtension(csProjFileName, "dll");

            if (File.Exists(binDebugPath + assemblyFileName)) return binDebugPath + assemblyFileName;
            if (File.Exists(binPath + assemblyFileName)) return binPath + assemblyFileName;

            string assemblyFileNameExe = Utilities.ChangeFileExtension(csProjFileName, "exe");

            if (File.Exists(binDebugPath + assemblyFileNameExe)) return binDebugPath + assemblyFileNameExe;
            if (File.Exists(binPath + assemblyFileNameExe)) return binPath + assemblyFileNameExe;

            throw new CliException($"None of: '{assemblyFileName}' or '{assemblyFileNameExe}' found in the default assembly folder (the project's bin\\Debug or bin\\ folders). Please make sure you have your project built.");
        }

        private static ConfigParams CreateConfigParams(string configPath)
        {
            if (!File.Exists(configPath))
            {
                if (verbose) Console.WriteLine("No config file found. Default configuration will be used.");
                return new ConfigParams();
            }

            if (verbose) Console.WriteLine($"Reading the config file from \"{configPath}\"");
            string configJson = File.ReadAllText(configPath);

            var serializer = new DataContractJsonSerializer(typeof(ConfigParams));
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(configJson));
            return (ConfigParams)serializer.ReadObject(stream);
        }

        private static string GetConfigPath(string[] args)
        {
            List<string> argsList = args.ToList();
            int index = argsList.IndexOf("-Config-Path");

            if (index < 0) return null;

            if (args.Length < index + 2) // index of the next element + 1
            {
                throw new CliException("-Config-Path parameter present, but no path specified");
            }

            return args[index + 1].NormalizePath();
        }
    }
}
