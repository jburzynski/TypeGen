using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Cli.Models;
using TypeGen.Core;
using TypeGen.Core.Converters;

namespace TypeGen.Cli.Business
{
    internal class GeneratorOptionsProvider
    {
        private readonly FileSystem _fileSystem;
        private readonly Logger _logger;

        public GeneratorOptionsProvider(FileSystem fileSystem, Logger logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        /// <summary>
        /// Returns the GeneratorOptions object based on the passed ConfigParams
        /// </summary>
        /// <param name="config"></param>
        /// <param name="assembly"></param>
        /// <param name="projectFolder"></param>
        /// <param name="logVerbose"></param>
        /// <returns></returns>
        public GeneratorOptions GetGeneratorOptions(TgConfig config, Assembly assembly, string projectFolder, bool logVerbose)
        {
            return new GeneratorOptions
            {
                TypeScriptFileExtension = config.TypeScriptFileExtension,
                TabLength = config.TabLength ?? GeneratorOptions.DefaultTabLength,
                ExplicitPublicAccessor = config.ExplicitPublicAccessor ?? GeneratorOptions.DefaultExplicitPublicAccessor,
                FileNameConverters = GetTypeNameConvertersFromConfig(config.FileNameConverters, assembly, projectFolder, logVerbose),
                TypeNameConverters = GetTypeNameConvertersFromConfig(config.TypeNameConverters, assembly, projectFolder, logVerbose),
                PropertyNameConverters = GetNameConvertersFromConfig(config.PropertyNameConverters, assembly, projectFolder, logVerbose),
                EnumValueNameConverters = GetNameConvertersFromConfig(config.EnumValueNameConverters, assembly, projectFolder, logVerbose)
            };
        }

        private TypeNameConverterCollection GetTypeNameConvertersFromConfig(string[] typeNameConverters, Assembly assembly, string projectFolder, bool logVerbose)
        {
            IEnumerable<ITypeNameConverter> converters = typeNameConverters.Select(name => GetConverterFromName<ITypeNameConverter>(name, assembly, projectFolder, logVerbose));
            return new TypeNameConverterCollection(converters);
        }

        private NameConverterCollection GetNameConvertersFromConfig(string[] nameConverters, Assembly assembly, string projectFolder, bool logVerbose)
        {
            IEnumerable<INameConverter> converters = nameConverters.Select(name => GetConverterFromName<INameConverter>(name, assembly, projectFolder, logVerbose));
            return new NameConverterCollection(converters);
        }

        private TConverter GetConverterFromName<TConverter>(string name, Assembly assembly, string projectFolder, bool logVerbose) where TConverter : class, IConverter
        {
            string[] nameParts = name.Split(':');

            if (nameParts.Length == 1)
            {
                return GetConverterNoAssembly<TConverter>(name, assembly, logVerbose);
            }

            if (nameParts.Length == 2)
            {
                return GetConverterFromAssembly<TConverter>(nameParts[0], nameParts[1], projectFolder);
            }

            throw new CliException($"Failed to load converter '{name}'. Incorrect name format.");
        }

        private TConverter GetConverterFromAssembly<TConverter>(string assemblyPath, string name, string projectFolder) where TConverter : class, IConverter
        {
            string assemblyFullPath = $"{projectFolder}\\{assemblyPath}";
            if (!_fileSystem.FileExists(assemblyFullPath))
            {
                throw new CliException($"Assembly path '{assemblyFullPath}' not found for converter '{name}'");
            }

            Assembly converterAssembly = Assembly.LoadFrom(assemblyFullPath);
            return GetConverterFromAssembly<TConverter>(converterAssembly, name);
        }

        private TConverter GetConverterFromAssembly<TConverter>(Assembly converterAssembly, string converterName) where TConverter : class, IConverter
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

        private TConverter GetConverterNoAssembly<TConverter>(string name, Assembly assembly, bool logVerbose) where TConverter : class, IConverter
        {
            // first, try to get the converter from the current loaded assembly
            var result = GetConverterFromAssembly<TConverter>(assembly, name);
            if (result != null)
            {
                if (logVerbose) _logger.Log($"Converter '{name}' found in assembly '{assembly.FullName}'");
                return result;
            }

            if (logVerbose) _logger.Log($"Converter '{name}' not found in assembly '{assembly.FullName}'. Falling back to TypeGen.Core.");

            // fallback to TypeGen.Core
            Assembly coreAssembly = typeof(Generator).Assembly;
            result = GetConverterFromAssembly<TConverter>(coreAssembly, name);
            if (result != null)
            {
                if (logVerbose) _logger.Log($"Converter '{name}' found in TypeGen.Core");
                return result;
            }

            throw new CliException($"Converter '{name}' not found in assembly '{assembly.FullName}' or TypeGen.Core");
        }
    }
}
