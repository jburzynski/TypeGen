using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeGen.Core.Converters;
using TypeGen.Core.Extensions;

namespace TypeGen.Core
{
    /// <summary>
    /// Options for generating TypeScript files
    /// </summary>
    public class GeneratorOptions
    {
        public static int DefaultTabLength => 4;
        public static bool DefaultExplicitPublicAccessor => false;
        public static TypeNameConverterCollection DefaultFileNameConverters => new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter());
        public static TypeNameConverterCollection DefaultTypeNameConverters => new TypeNameConverterCollection();
        public static NameConverterCollection DefaultPropertyNameConverters => new NameConverterCollection(new PascalCaseToCamelCaseConverter());
        public static NameConverterCollection DefaultEnumValueNameConverters => new NameConverterCollection();
        public static string DefaultTypeScriptFileExtension => "ts";
        public static bool DefaultSingleQuotes => false;

        public GeneratorOptions()
        {
            FileNameConverters = DefaultFileNameConverters;
            TypeNameConverters = DefaultTypeNameConverters;
            PropertyNameConverters = DefaultPropertyNameConverters;
            EnumValueNameConverters = DefaultEnumValueNameConverters;
            TypeScriptFileExtension = DefaultTypeScriptFileExtension;
            TabLength = DefaultTabLength;
            ExplicitPublicAccessor = DefaultExplicitPublicAccessor;
            SingleQuotes = DefaultSingleQuotes;
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
        public NameConverterCollection PropertyNameConverters { get; set; }

        /// <summary>
        /// A collection (chain) of converters used for converting C# enum value names to TypeScript enum value names
        /// </summary>
        public NameConverterCollection EnumValueNameConverters { get; set; }

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

        private string _baseOutputDirectory;

        /// <summary>
        /// The base directory for generating TypeScript files.
        /// Any relative paths defined in ExportTs... attributes (OutputDir) will be resolved relatively to this path.
        /// </summary>
        public string BaseOutputDirectory
        {
            get { return _baseOutputDirectory; }
            set { _baseOutputDirectory = value.NormalizePath(); }
        }
    }
}
