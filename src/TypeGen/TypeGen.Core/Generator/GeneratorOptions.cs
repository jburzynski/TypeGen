using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TypeGen.Core.Converters;
using TypeGen.Core.Extensions;
using TypeGen.Core.Generator.Services;
using TypeGen.Core.Utils;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Generator
{
    /// <summary>
    /// Options for generating TypeScript files
    /// </summary>
    public class GeneratorOptions
    {
        public static int DefaultTabLength => 4;
        public static bool DefaultUseTabCharacter => false;
        public static bool DefaultExplicitPublicAccessor => false;
        public static TypeNameConverterCollection DefaultFileNameConverters => new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter());
        public static TypeNameConverterCollection DefaultTypeNameConverters => new TypeNameConverterCollection();
        public static MemberNameConverterCollection DefaultPropertyNameConverters => new MemberNameConverterCollection(new PascalCaseToCamelCaseConverter());
        public static MemberNameConverterCollection DefaultEnumValueNameConverters => new MemberNameConverterCollection();
        public static MemberNameConverterCollection DefaultEnumStringInitializersConverters => new MemberNameConverterCollection();
        public static string DefaultTypeScriptFileExtension => "ts";
        public static bool DefaultSingleQuotes => false;
        public static bool DefaultCreateIndexFile => false;
        public static StrictNullTypeUnionFlags DefaultCsNullableTranslation => StrictNullTypeUnionFlags.None;
        public static bool DefaultCsAllowNullsForAllTypes = false;
        public static bool DefaultCsDefaultValuesForConstantsOnly = false;
        public static IDictionary<string, string> DefaultDefaultValuesForTypes => new Dictionary<string, string>();
        public static IDictionary<string, IEnumerable<string>> DefaultTypeUnionsForTypes => new Dictionary<string, IEnumerable<string>>();
        public static IDictionary<string, string> DefaultCustomTypeMappings => new Dictionary<string, string>();
        public static bool DefaultEnumStringInitializers => false;
        public static string DefaultFileHeading => null;
        public static bool DefaultUseDefaultExport => false;
        public static string DefaultIndexFileExtension => DefaultTypeScriptFileExtension;
        public static bool DefaultExportTypesAsInterfacesByDefault => false;
        public static bool DefaultUseImportType => false;

        public static HashSet<string> DefaultTypeBlacklist => new(new []
        {
            "System.IAsyncDisposable",
            typeof(ICloneable).FullName,
            typeof(IComparable).FullName,
            typeof(IComparable<>).FullName,
            typeof(IConvertible).FullName,
            typeof(IDisposable).FullName,
            typeof(IEquatable<>).FullName,
            typeof(IFormattable).FullName,
            "System.IParsable`1",
            typeof(ISerializable).FullName,
            "System.ISpanFormattable",
            "System.ISpanParsable`1",
            typeof(ValueType).FullName
        });

        /// <summary>
        /// A collection (chain) of converters used for converting C# file names to TypeScript file names
        /// </summary>
        public TypeNameConverterCollection FileNameConverters { get; set; } = DefaultFileNameConverters;

        /// <summary>
        /// A collection (chain) of converters used for converting C# type names (classes, enums etc.) to TypeScript type names
        /// </summary>
        public TypeNameConverterCollection TypeNameConverters { get; set; } = DefaultTypeNameConverters;

        /// <summary>
        /// A collection (chain) of converters used for converting C# property names to TypeScript property names
        /// </summary>
        public MemberNameConverterCollection PropertyNameConverters { get; set; } = DefaultPropertyNameConverters;

        /// <summary>
        /// A collection (chain) of converters used for converting C# enum value names to TypeScript enum value names
        /// </summary>
        public MemberNameConverterCollection EnumValueNameConverters { get; set; } = DefaultEnumValueNameConverters;

        /// <summary>
        /// A collection (chain) of converters used for converting C# enum value names to TypeScript enum string initializers
        /// </summary>
        public MemberNameConverterCollection EnumStringInitializersConverters { get; set; } = DefaultEnumStringInitializersConverters;

        /// <summary>
        /// Whether to generate explicit "public" accessor in TypeScript classes
        /// </summary>
        public bool ExplicitPublicAccessor { get; set; } = DefaultExplicitPublicAccessor;

        /// <summary>
        /// Whether to use single quotes instead of double quotes in TypeScript sources
        /// </summary>
        public bool SingleQuotes { get; set; } = DefaultSingleQuotes;

        /// <summary>
        /// File extension used for the generated TypeScript files
        /// </summary>
        public string TypeScriptFileExtension { get; set; } = DefaultTypeScriptFileExtension;

        /// <summary>
        /// Number of space characters per tab
        /// </summary>
        public int TabLength { get; set; } = DefaultTabLength;

        /// <summary>
        /// Whether to use the tab character instead of multiple spaces
        /// </summary>
        public bool UseTabCharacter { get; set; } = DefaultUseTabCharacter;

        private string _baseOutputDirectory;

        /// <summary>
        /// The base directory for generating TypeScript files.
        /// Any relative paths defined in ExportTs... attributes (OutputDir) will be resolved relatively to this path.
        /// </summary>
        public string BaseOutputDirectory
        {
            get => _baseOutputDirectory;
            set => _baseOutputDirectory = FileSystemUtils.AsDirectory(value);
        }

        /// <summary>
        /// Whether to create an index file which exports all generated types
        /// </summary>
        public bool CreateIndexFile { get; set; } = DefaultCreateIndexFile;

        /// <summary>
        /// Indicates which union types (null, undefined) are added to TypeScript property types for C# nullable types by default
        /// </summary>
        public StrictNullTypeUnionFlags CsNullableTranslation { get; set; } = DefaultCsNullableTranslation;

        /// <summary>
        /// Specifies whether null union types should be added for all types
        /// </summary>
        public bool CsAllowNullsForAllTypes { get; set; } = DefaultCsAllowNullsForAllTypes;

        /// <summary>
        /// Specifies that only default values for constants are generated
        /// </summary>
        public bool CsDefaultValuesForConstantsOnly { get; set; } = DefaultCsDefaultValuesForConstantsOnly;

        /// <summary>
        /// Specifies default values to generate for given TypeScript types
        /// </summary>
        public IDictionary<string, string> DefaultValuesForTypes { get; set; } = DefaultDefaultValuesForTypes;

        /// <summary>
        /// Specifies TypeScript type unions (excluding the main type) for TypeScript properties of specified types
        /// </summary>
        public IDictionary<string, IEnumerable<string>> TypeUnionsForTypes { get; set; } = DefaultTypeUnionsForTypes;

        /// <summary>
        /// Custom [C# -> TS] type mappings. C# type name must be a full type name (e.g. "SomeNs.My.Type").
        /// Specified C# types will be always translated to the corresponding TypeScript types.
        /// </summary>
        public IDictionary<string, string> CustomTypeMappings { get; set; } = DefaultCustomTypeMappings;

        /// <summary>
        /// Indicates whether to use enum string initializers
        /// </summary>
        public bool EnumStringInitializers { get; set; } = DefaultEnumStringInitializers;

        /// <summary>
        /// Heading section (initial section) of a TypeScript file. By default it's "This is a TypeGen auto-generated file. (...)"
        /// </summary>
        public string FileHeading { get; set; } = DefaultFileHeading;

        /// <summary>
        /// Whether to use default exports for the generated TypeScript types
        /// </summary>
        public bool UseDefaultExport { get; set; } = DefaultUseDefaultExport;

        /// <summary>
        /// The file extension to use for the index file(s). Defaults to whatever is set for TypeScriptFileExtension.
        /// </summary>
        public string IndexFileExtension { get; set; } = DefaultIndexFileExtension;

        /// <summary>
        /// Whether to export types as interfaces by default. For example affects member types which aren't explicitly selected to be generated.
        /// </summary>
        public bool ExportTypesAsInterfacesByDefault { get; set; } = DefaultExportTypesAsInterfacesByDefault;

        /// <summary>
        /// Whether to use "import type" instead of "import" for imports in TS sources.
        /// </summary>
        public bool UseImportType { get; set; } = DefaultUseImportType;

        /// <summary>
        /// Specifies types which should not be generated.
        /// </summary>
        public HashSet<string> TypeBlacklist { get; set; } = DefaultTypeBlacklist;

        /// <summary>
        /// Checks if the type is on the type blacklist.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>true if the type is on the blacklist, false otherwise</returns>
        public bool IsTypeBlacklisted(Type type)
        {
            Requires.NotNull(type, nameof(type));

            if (type.IsGenericParameter) return false;

            var nameWithNamespace = $"{type.Namespace}.{type.Name}";
            
            return TypeBlacklist.Contains(nameWithNamespace.RemoveGenericArgumentsFromTypeName())
                   || TypeBlacklist.Contains(type.Name.RemoveGenericArgumentsFromTypeName());
        }

        /// <summary>
        /// Checks if the type is not on the type blacklist.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>true if the type is not on the blacklist, false otherwise</returns>
        public bool IsTypeNotBlacklisted(Type type) => !IsTypeBlacklisted(type);
    }
}
