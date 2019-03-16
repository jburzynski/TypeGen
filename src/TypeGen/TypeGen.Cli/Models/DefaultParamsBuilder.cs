using System.Linq;
using TypeGen.Cli.Extensions;
using TypeGen.Core;
using TypeGen.Core.Extensions;

namespace TypeGen.Cli.Models
{
	internal class DefaultParamsBuilder
	{
		private readonly TgConfig _config;

		private DefaultParamsBuilder(TgConfig config)
		{
			_config = config;
		}

		public static DefaultParamsBuilder InstanceFor(TgConfig config)
		{
			return new DefaultParamsBuilder(config);
		}

		public DefaultParamsBuilder WithAssembliesOrDefault()
		{
			if (_config.Assemblies == null)
			{
				_config.Assemblies = new string[0];
			}

			return this;
		}

		public DefaultParamsBuilder WithGenerationSpecsOrDefault()
		{
			if (_config.GenerationSpecs == null)
			{
				_config.GenerationSpecs = new string[0];
			}

			return this;
		}

		public DefaultParamsBuilder WithExplicitPublicAccessorOrDefault()
		{
			if (_config.ExplicitPublicAccessor == null)
			{
				_config.ExplicitPublicAccessor = GeneratorOptions.DefaultExplicitPublicAccessor;
			}

			return this;
		}

		public DefaultParamsBuilder WithSingleQuotesOrDefault()
		{
			if (_config.SingleQuotes == null)
			{
				_config.SingleQuotes = GeneratorOptions.DefaultSingleQuotes;
			}

			return this;
		}

		public DefaultParamsBuilder WithDefaultAddFilesToProjectOrDefault()
		{
			if (_config.AddFilesToProject == null)
			{
				_config.AddFilesToProject = TgConfig.DefaultAddFilesToProject;
			}

			return this;
		}

		public DefaultParamsBuilder WithTypeScriptFileExtensionOrDefault()
		{
			if (_config.TypeScriptFileExtension == null)
			{
				_config.TypeScriptFileExtension = GeneratorOptions.DefaultTypeScriptFileExtension;
			}

			return this;
		}

		public DefaultParamsBuilder WithTabLengthOrDefault()
		{
			if (_config.TabLength == null)
			{
				_config.TabLength = GeneratorOptions.DefaultTabLength;
			}

			return this;
		}

		public DefaultParamsBuilder WithFileNameConvertersOrDefault()
		{
			if (_config.FileNameConverters == null)
			{
				_config.FileNameConverters = GeneratorOptions.DefaultFileNameConverters.GetTypeNames().ToArray();
			}

			return this;
		}

		public DefaultParamsBuilder WithTypeNameConvertersOrDefault()
		{
			if (_config.TypeNameConverters == null)
			{
				_config.TypeNameConverters = GeneratorOptions.DefaultTypeNameConverters.GetTypeNames().ToArray();
			}

			return this;
		}

		public DefaultParamsBuilder WithPropertyNameConvertersOrDefault()
		{
			if (_config.PropertyNameConverters == null)
			{
				_config.PropertyNameConverters = GeneratorOptions.DefaultPropertyNameConverters.GetTypeNames().ToArray();
			}

			return this;
		}

		public DefaultParamsBuilder WithEnumValueNameConvertersOrDefault()
		{ 
			if (_config.EnumValueNameConverters == null)
			{
				_config.EnumValueNameConverters = GeneratorOptions.DefaultEnumValueNameConverters.GetTypeNames().ToArray();
			}

			return this;
		}

		public DefaultParamsBuilder WithEnumStringInitializersConvertersOrDefault()
		{
			if (_config.EnumStringInitializersConverters == null)
			{
				_config.EnumStringInitializersConverters = GeneratorOptions.DefaultEnumStringInitializersConverters.GetTypeNames().ToArray();
			}

			return this;
		}

		public DefaultParamsBuilder WithExternalAssemblyPathsOrDefault()
		{
			if (_config.ExternalAssemblyPaths == null)
			{
				_config.ExternalAssemblyPaths = new string[0];
			}

			return this;
		}

		public DefaultParamsBuilder WithCreateIndexFileOrDefault()
		{
			if (_config.CreateIndexFile == null)
			{
				_config.CreateIndexFile = GeneratorOptions.DefaultCreateIndexFile;
			}

			return this;
		}

		public DefaultParamsBuilder WithStrictNullChecksOrDefault()
		{
			if (_config.StrictNullChecks == null)
			{
				_config.StrictNullChecks = GeneratorOptions.DefaultStrictNullChecks;
			}

			return this;
		}

		public DefaultParamsBuilder WithCsNullableTranslationOrDefault()
		{ 
			if (_config.CsNullableTranslation == null)
			{
				_config.CsNullableTranslation = GeneratorOptions.DefaultCsNullableTranslation.ToFlagString();
			}

			return this;
		}

		public DefaultParamsBuilder WithOutputPathOrDefault()
		{
			if (_config.OutputPath == null)
			{
				_config.OutputPath = string.Empty;
			}

			return this;
		}

		public DefaultParamsBuilder WithClearOutputDirectoryOrDefault()
		{
			if (_config.ClearOutputDirectory == null)
			{
				_config.ClearOutputDirectory = false;
			}

			return this;
		}

		public DefaultParamsBuilder WithDefaultValuesForTypesOrDefault()
		{
			if (_config.DefaultValuesForTypes == null)
			{
				_config.DefaultValuesForTypes = GeneratorOptions
					.DefaultDefaultValuesForTypes
					.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			}

			return this;
		}

		public DefaultParamsBuilder WithCustomTypeMappingsOrDefault()
		{
			if (_config.CustomTypeMappings == null)
			{
				_config.CustomTypeMappings = GeneratorOptions
					.DefaultCustomTypeMappings
					.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			}

			return this;
		}

		public DefaultParamsBuilder WithUseAttributesWithGenerationSpecOrDefault()
		{
			if (_config.UseAttributesWithGenerationSpec == null)
			{
				_config.UseAttributesWithGenerationSpec = GeneratorOptions.DefaultUseAttributesWithGenerationSpec;
			}

			return this;
		}

		public DefaultParamsBuilder WithEnumStringInitializersOrDefault()
		{
			if (_config.EnumStringInitializers == null)
			{
				_config.EnumStringInitializers = GeneratorOptions.DefaultEnumStringInitializers;
			}

			return this;
		}

		public DefaultParamsBuilder WithFileHeadingOrDefault()
		{
			if (_config.FileHeading == null)
			{
				_config.FileHeading = GeneratorOptions.DefaultFileHeading;
			}

			return this;
		}
		public DefaultParamsBuilder WithGenerateFromAssembliesOrDefault()
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