using System.Collections.Generic;
using TypeGen.Core.Converters;
using TypeGen.Core.Generator.Services;
using TypeGen.Core.Utils;

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
        public static IDictionary<string, string> DefaultDefaultValuesForTypes => new Dictionary<string, string>();
        public static IDictionary<string, IEnumerable<string>> DefaultTypeUnionsForTypes => new Dictionary<string, IEnumerable<string>>();
        public static IDictionary<string, string> DefaultCustomTypeMappings => new Dictionary<string, string>();
        public static bool DefaultEnumStringInitializers => false;
        public static string DefaultFileHeading => null;
        public static bool DefaultUseDefaultExport => false;
        public static string DefaultIndexFileExtension => DefaultTypeScriptFileExtension;

        public GeneratorOptions()
        {
            TabLength = DefaultTabLength;
            UseTabCharacter = DefaultUseTabCharacter;
            ExplicitPublicAccessor = DefaultExplicitPublicAccessor;
            FileNameConverters = DefaultFileNameConverters;
            TypeNameConverters = DefaultTypeNameConverters;
            PropertyNameConverters = DefaultPropertyNameConverters;
            EnumValueNameConverters = DefaultEnumValueNameConverters;
            EnumStringInitializersConverters = DefaultEnumStringInitializersConverters;
            TypeScriptFileExtension = DefaultTypeScriptFileExtension;
            SingleQuotes = DefaultSingleQuotes;
            CreateIndexFile = DefaultCreateIndexFile;
            CsNullableTranslation = DefaultCsNullableTranslation;
            CsAllowNullsForAllTypes = DefaultCsAllowNullsForAllTypes;
            DefaultValuesForTypes = DefaultDefaultValuesForTypes;
            TypeUnionsForTypes = DefaultTypeUnionsForTypes;
            CustomTypeMappings = DefaultCustomTypeMappings;
            EnumStringInitializers = DefaultEnumStringInitializers;
            FileHeading = DefaultFileHeading;
            UseDefaultExport = DefaultUseDefaultExport;
            IndexFileExtension = DefaultIndexFileExtension;
        }

        /// <summary>
        /// A collection (chain) of converters used for converting C# file names to TypeScript file names
        /// </summary>
        public TypeNameConverterCollection FileNameConverters { get; set; }

        /// <summary>
        /// A collection (chain) of converters used for converting C# type names (classes, enums etc.) to TypeScript type names
        /// </summary>
        public TypeNameConverterCollection TypeNameConverters { get; set; }

        /// <summary>
        /// A collection (chain) of converters used for converting C# class property names to TypeScript class property names
        /// </summary>
        public MemberNameConverterCollection PropertyNameConverters { get; set; }

        /// <summary>
        /// A collection (chain) of converters used for converting C# enum value names to TypeScript enum value names
        /// </summary>
        public MemberNameConverterCollection EnumValueNameConverters { get; set; }
        
        /// <summary>
        /// A collection (chain) of converters used for converting C# enum value names to TypeScript enum string initializers
        /// </summary>
        public MemberNameConverterCollection EnumStringInitializersConverters { get; set; }

        /// <summary>
        /// Whether to generate explicit "public" accessor in TypeScript classes
        /// </summary>
        public bool ExplicitPublicAccessor { get; set; }

        /// <summary>
        /// Whether to use single quotes instead of double quotes in TypeScript sources
        /// </summary>
        public bool SingleQuotes { get; set; }

        /// <summary>
        /// File extension used for the generated TypeScript files
        /// </summary>
        public string TypeScriptFileExtension { get; set; }

        /// <summary>
        /// Number of space characters per tab
        /// </summary>
        public int TabLength { get; set; }
        
        /// <summary>
        /// Whether to use the tab character instead of multiple spaces
        /// </summary>
        public bool UseTabCharacter { get; set; }

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
        public bool CreateIndexFile { get; set; }

        /// <summary>
        /// Indicates which union types (null, undefined) are added to TypeScript property types for C# nullable types by default
        /// </summary>
        public StrictNullTypeUnionFlags CsNullableTranslation { get; set; }

        /// <summary>
        /// Indicates to allow null for all types.
        /// </summary>
        public bool CsAllowNullsForAllTypes { get; set; }

        /// <summary>
        /// Specifies default values to generate for given TypeScript types
        /// </summary>
        public IDictionary<string, string> DefaultValuesForTypes { get; set; }
        
        /// <summary>
        /// Specifies TypeScript type unions (excluding the main type) for TypeScript properties of specified types
        /// </summary>
        public IDictionary<string, IEnumerable<string>> TypeUnionsForTypes { get; set; }

        /// <summary>
        /// Custom [C# -> TS] type mappings. C# type name must be a full type name (e.g. "SomeNs.My.Type").
        /// Specified C# types will be always translated to the corresponding TypeScript types.
        /// </summary>
        public IDictionary<string, string> CustomTypeMappings { get; set; }

        /// <summary>
        /// Indicates whether to use enum string initializers
        /// </summary>
        public bool EnumStringInitializers { get; set; }

        /// <summary>
        /// Heading section (initial section) of a TypeScript file. By default it's "This is a TypeGen auto-generated file. (...)"
        /// </summary>
        public string FileHeading { get; set; }
        
        /// <summary>
        /// Whether to use default exports for the generated TypeScript types
        /// </summary>
        public bool UseDefaultExport { get; set; }

        /// <summary>
        /// The file extension to use for the index file(s). Defaults to whatever is set for TypeScriptFileExtension.
        /// </summary>
        public string IndexFileExtension { get; set; }
    }
}
