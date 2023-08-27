using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Cli.Extensions;
using TypeGen.Cli.TypeResolution;
using TypeGen.Core.Converters;
using TypeGen.Core.Generator;
using TypeGen.Core.Validation;

namespace TypeGen.Cli.GenerationConfig
{
    internal class GeneratorOptionsProvider : IGeneratorOptionsProvider
    {
        private readonly IConverterResolver _converterResolver;

        public GeneratorOptionsProvider(IConverterResolver converterResolver)
        {
            _converterResolver = converterResolver;
        }

        /// <summary>
        /// Returns the GeneratorOptions object based on the passed ConfigParams
        /// </summary>
        /// <param name="config"></param>
        /// <param name="assemblies"></param>
        /// <param name="projectFolder"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public GeneratorOptions GetGeneratorOptions(TgConfig config, IEnumerable<Assembly> assemblies, string projectFolder)
        {
            Requires.NotNull(config, nameof(config));
            Requires.NotNull(assemblies, nameof(assemblies));
            Requires.NotNullOrEmpty(projectFolder, nameof(projectFolder));
            
            return new GeneratorOptions
            {
                BaseOutputDirectory = config.OutputPath.RelativeOrRooted(projectFolder),
                TypeScriptFileExtension = config.TypeScriptFileExtension,
                TabLength = config.TabLength ?? GeneratorOptions.DefaultTabLength,
                UseTabCharacter = config.UseTabCharacter ?? GeneratorOptions.DefaultUseTabCharacter,
                ExplicitPublicAccessor = config.ExplicitPublicAccessor ?? GeneratorOptions.DefaultExplicitPublicAccessor,
                SingleQuotes = config.SingleQuotes ?? GeneratorOptions.DefaultSingleQuotes,
                FileNameConverters = GetTypeNameConvertersFromConfig(config.FileNameConverters),
                TypeNameConverters = GetTypeNameConvertersFromConfig(config.TypeNameConverters),
                PropertyNameConverters = GetMemberNameConvertersFromConfig(config.PropertyNameConverters),
                EnumValueNameConverters = GetMemberNameConvertersFromConfig(config.EnumValueNameConverters),
                EnumStringInitializersConverters = GetMemberNameConvertersFromConfig(config.EnumStringInitializersConverters),
                CsNullableTranslation = config.CsNullableTranslation.ToStrictNullFlags(),
                CsAllowNullsForAllTypes = config.CsAllowNullsForAllTypes ?? GeneratorOptions.DefaultCsAllowNullsForAllTypes,
                CsDefaultValuesForConstantsOnly = config.CsDefaultValuesForConstantsOnly ?? GeneratorOptions.DefaultCsDefaultValuesForConstantsOnly,
                CreateIndexFile = config.CreateIndexFile ?? GeneratorOptions.DefaultCreateIndexFile,
                DefaultValuesForTypes = config.DefaultValuesForTypes ?? GeneratorOptions.DefaultDefaultValuesForTypes,
                TypeUnionsForTypes = config.TypeUnionsForTypes ?? GeneratorOptions.DefaultTypeUnionsForTypes,
                CustomTypeMappings = config.CustomTypeMappings ?? GeneratorOptions.DefaultCustomTypeMappings,
                EnumStringInitializers = config.EnumStringInitializers ?? GeneratorOptions.DefaultEnumStringInitializers,
                FileHeading = config.FileHeading ?? GeneratorOptions.DefaultFileHeading,
                UseDefaultExport = config.UseDefaultExport ?? GeneratorOptions.DefaultUseDefaultExport,
                ExportTypesAsInterfacesByDefault = config.ExportTypesAsInterfacesByDefault ?? GeneratorOptions.DefaultExportTypesAsInterfacesByDefault,
                UseImportType = config.UseImportType ?? GeneratorOptions.DefaultUseImportType,
                TypeBlacklist = GetTypeBlacklist(config.TypeBlacklist, config.TypeWhitelist)
            };
        }

        private static HashSet<string> GetTypeBlacklist(string[] configTypeBlacklist, string[] configTypeWhitelist)
        {
            var defaultBlacklist = GeneratorOptions.DefaultTypeBlacklist.ToArray();
            var blacklist = defaultBlacklist.Concat(configTypeBlacklist).Except(configTypeWhitelist);
            return new HashSet<string>(blacklist);
        }

        private TypeNameConverterCollection GetTypeNameConvertersFromConfig(IEnumerable<string> typeNameConverters)
        {
            var converters = _converterResolver.Resolve<ITypeNameConverter>(typeNameConverters);
            return new TypeNameConverterCollection(converters);
        }

        private MemberNameConverterCollection GetMemberNameConvertersFromConfig(IEnumerable<string> nameConverters)
        {
            IEnumerable<IMemberNameConverter> converters = _converterResolver.Resolve<IMemberNameConverter>(nameConverters);
            return new MemberNameConverterCollection(converters);
        }
    }
}
