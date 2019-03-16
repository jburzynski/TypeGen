using System.Linq;
using TypeGen.Cli.Extensions;
using TypeGen.Core;
using TypeGen.Core.Extensions;

namespace TypeGen.Cli.Models
{
	internal class DefaultParamsMerger
	{
		private readonly TgConfig _config;

		private DefaultParamsMerger(TgConfig config)
		{
			_config = config;
		}

		public static DefaultParamsMerger InstanceFor(TgConfig config)
		{
			return new DefaultParamsMerger(config);
		}

		public DefaultParamsMerger WithAssembliesOrDefault()
		{
			if (_config.Assemblies == null)
			{
				_config.Assemblies = new string[0];
			}

			return this;
		}

		public DefaultParamsMerger WithGenerationSpecsOrDefault()
		{
			if (_config.GenerationSpecs == null)
			{
				_config.GenerationSpecs = new string[0];
			}

			return this;
		}

		public DefaultParamsMerger WithExplicitPublicAccessorOrDefault()
		{
			if (_config.ExplicitPublicAccessor == null)
			{
				_config.ExplicitPublicAccessor = GeneratorOptions.DefaultExplicitPublicAccessor;
			}

			return this;
		}

		public DefaultParamsMerger WithSingleQuotesOrDefault()
		{
			if (_config.SingleQuotes == null)
			{
				_config.SingleQuotes = GeneratorOptions.DefaultSingleQuotes;
			}

			return this;
		}

		public DefaultParamsMerger WithDefaultAddFilesToProjectOrDefault()
		{
			if (_config.AddFilesToProject == null)
			{
				_config.AddFilesToProject = TgConfig.DefaultAddFilesToProject;
			}

			return this;
		}

		public DefaultParamsMerger WithTypeScriptFileExtensionOrDefault()
		{
			if (_config.TypeScriptFileExtension == null)
			{
				_config.TypeScriptFileExtension = GeneratorOptions.DefaultTypeScriptFileExtension;
			}

			return this;
		}

		public DefaultParamsMerger WithTabLengthOrDefault()
		{
			if (_config.TabLength == null)
			{
				_config.TabLength = GeneratorOptions.DefaultTabLength;
			}

			return this;
		}

		public DefaultParamsMerger WithFileNameConvertersOrDefault()
		{
			if (_config.FileNameConverters == null)
			{
				_config.FileNameConverters = GeneratorOptions.DefaultFileNameConverters.GetTypeNames().ToArray();
			}

			return this;
		}

		public DefaultParamsMerger WithTypeNameConvertersOrDefault()
		{
			if (_config.TypeNameConverters == null)
			{
				_config.TypeNameConverters = GeneratorOptions.DefaultTypeNameConverters.GetTypeNames().ToArray();
			}

			return this;
		}

		public DefaultParamsMerger WithPropertyNameConvertersOrDefault()
		{
			if (_config.PropertyNameConverters == null)
			{
				_config.PropertyNameConverters = GeneratorOptions.DefaultPropertyNameConverters.GetTypeNames().ToArray();
			}

			return this;
		}

		public DefaultParamsMerger WithEnumValueNameConvertersOrDefault()
		{ 
			if (_config.EnumValueNameConverters == null)
			{
				_config.EnumValueNameConverters = GeneratorOptions.DefaultEnumValueNameConverters.GetTypeNames().ToArray();
			}

			return this;
		}

		public DefaultParamsMerger WithEnumStringInitializersConvertersOrDefault()
		{
			if (_config.EnumStringInitializersConverters == null)
			{
				_config.EnumStringInitializersConverters = GeneratorOptions.DefaultEnumStringInitializersConverters.GetTypeNames().ToArray();
			}

			return this;
		}

		public DefaultParamsMerger WithExternalAssemblyPathsOrDefault()
		{
			if (_config.ExternalAssemblyPaths == null)
			{
				_config.ExternalAssemblyPaths = new string[0];
			}

			return this;
		}

		public DefaultParamsMerger WithCreateIndexFileOrDefault()
		{
			if (_config.CreateIndexFile == null)
			{
				_config.CreateIndexFile = GeneratorOptions.DefaultCreateIndexFile;
			}

			return this;
		}

		public DefaultParamsMerger WithStrictNullChecksOrDefault()
		{
			if (_config.StrictNullChecks == null)
			{
				_config.StrictNullChecks = GeneratorOptions.DefaultStrictNullChecks;
			}

			return this;
		}

		public DefaultParamsMerger WithCsNullableTranslationOrDefault()
		{ 
			if (_config.CsNullableTranslation == null)
			{
				_config.CsNullableTranslation = GeneratorOptions.DefaultCsNullableTranslation.ToFlagString();
			}

			return this;
		}

		public DefaultParamsMerger WithOutputPathOrDefault()
		{
			if (_config.OutputPath == null)
			{
				_config.OutputPath = string.Empty;
			}

			return this;
		}

		public DefaultParamsMerger WithClearOutputDirectoryOrDefault()
		{
			if (_config.ClearOutputDirectory == null)
			{
				_config.ClearOutputDirectory = false;
			}

			return this;
		}

		public DefaultParamsMerger WithDefaultValuesForTypesOrDefault()
		{
			if (_config.DefaultValuesForTypes == null)
			{
				_config.DefaultValuesForTypes = GeneratorOptions
					.DefaultDefaultValuesForTypes
					.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			}

			return this;
		}

		public DefaultParamsMerger WithCustomTypeMappingsOrDefault()
		{
			if (_config.CustomTypeMappings == null)
			{
				_config.CustomTypeMappings = GeneratorOptions
					.DefaultCustomTypeMappings
					.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			}

			return this;
		}

		public DefaultParamsMerger WithUseAttributesWithGenerationSpecOrDefault()
		{
			if (_config.UseAttributesWithGenerationSpec == null)
			{
				_config.UseAttributesWithGenerationSpec = GeneratorOptions.DefaultUseAttributesWithGenerationSpec;
			}

			return this;
		}

		public DefaultParamsMerger WithEnumStringInitializersOrDefault()
		{
			if (_config.EnumStringInitializers == null)
			{
				_config.EnumStringInitializers = GeneratorOptions.DefaultEnumStringInitializers;
			}

			return this;
		}

		public DefaultParamsMerger WithFileHeadingOrDefault()
		{
			if (_config.FileHeading == null)
			{
				_config.FileHeading = GeneratorOptions.DefaultFileHeading;
			}

			return this;
		}
		public DefaultParamsMerger WithGenerateFromAssembliesOrDefault()
		{
			// GenerateFromAssemblies should stay null if no value is provided
			if (_config.GenerateFromAssemblies == null)
			{
				_config.GenerateFromAssemblies = null;
			}

			return this;
		}

		




		public TgConfig Build()
		{
			return _config;
		}

	}
}