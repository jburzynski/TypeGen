using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Cli.Extensions;
using TypeGen.Cli.Models;
using TypeGen.Core;
using TypeGen.Core.Converters;
using TypeGen.Core.Extensions;
using TypeGen.Core.Generator;
using TypeGen.Core.Generator.Services;
using TypeGen.Core.Logging;
using TypeGen.Core.Storage;
using TypeGen.Core.Validation;

namespace TypeGen.Cli.Business
{
    internal class GeneratorOptionsProvider : IGeneratorOptionsProvider
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger _logger;

        private TypeResolver _typeResolver;

        public GeneratorOptionsProvider(IFileSystem fileSystem, ILogger logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        /// <summary>
        /// Returns the GeneratorOptions object based on the passed ConfigParams
        /// </summary>
        /// <param name="config"></param>
        /// <param name="assemblies"></param>
        /// <param name="projectFolder"></param>
        /// <returns></returns>
        public GeneratorOptions GetGeneratorOptions(TgConfig config, IEnumerable<Assembly> assemblies, string projectFolder)
        {
            Requires.NotNull(config, nameof(config));
            Requires.NotNull(assemblies, nameof(assemblies));
            Requires.NotNullOrEmpty(projectFolder, nameof(projectFolder));
            
            _typeResolver = new TypeResolver(_logger, _fileSystem, projectFolder, assemblies);
            
            return new GeneratorOptions
            {
                TypeScriptFileExtension = config.TypeScriptFileExtension,
                TabLength = config.TabLength ?? GeneratorOptions.DefaultTabLength,
                UseTabCharacter = config.UseTabCharacter ?? GeneratorOptions.DefaultUseTabCharacter,
                ExplicitPublicAccessor = config.ExplicitPublicAccessor ?? GeneratorOptions.DefaultExplicitPublicAccessor,
                SingleQuotes = config.SingleQuotes ?? GeneratorOptions.DefaultSingleQuotes,
                FileNameConverters = GetTypeNameConvertersFromConfig(config.FileNameConverters),
                TypeNameConverters = GetTypeNameConvertersFromConfig(config.TypeNameConverters),
                PropertyNameConverters = GetNameConvertersFromConfig(config.PropertyNameConverters),
                EnumValueNameConverters = GetNameConvertersFromConfig(config.EnumValueNameConverters),
                EnumStringInitializersConverters = GetNameConvertersFromConfig(config.EnumStringInitializersConverters),
                CsNullableTranslation = config.CsNullableTranslation.ToStrictNullFlags(),
                CreateIndexFile = config.CreateIndexFile ?? GeneratorOptions.DefaultCreateIndexFile,
                DefaultValuesForTypes = config.DefaultValuesForTypes ?? GeneratorOptions.DefaultDefaultValuesForTypes,
                CustomTypeMappings = config.CustomTypeMappings ?? GeneratorOptions.DefaultCustomTypeMappings,
                EnumStringInitializers = config.EnumStringInitializers ?? GeneratorOptions.DefaultEnumStringInitializers,
                FileHeading = config.FileHeading ?? GeneratorOptions.DefaultFileHeading,
                UseDefaultExport = config.UseDefaultExport ?? GeneratorOptions.DefaultUseDefaultExport
            };
        }

        private TypeNameConverterCollection GetTypeNameConvertersFromConfig(IEnumerable<string> typeNameConverters)
        {
            IEnumerable<ITypeNameConverter> converters = GetConverters<ITypeNameConverter>(typeNameConverters);
            return new TypeNameConverterCollection(converters);
        }

        private NameConverterCollection GetNameConvertersFromConfig(IEnumerable<string> nameConverters)
        {
            IEnumerable<INameConverter> converters = GetConverters<INameConverter>(nameConverters);
            return new NameConverterCollection(converters);
        }

        private IEnumerable<T> GetConverters<T>(IEnumerable<string> converters)
        {
            return converters
                .Select(name => _typeResolver.Resolve(name, "Converter", new[] { typeof(T) }))
                .Where(t => t != null)
                .Select(t => (T)Activator.CreateInstance(t));
        }
    }
}
