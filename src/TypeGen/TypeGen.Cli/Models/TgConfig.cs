using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TypeGen.Core;
using TypeGen.Core.Converters;
using TypeGen.Core.Extensions;

namespace TypeGen.Cli.Models
{
    /// <summary>
    /// Represents console configuration
    /// </summary>
    [DataContract]
    internal class TgConfig
    {
        public TgConfig Normalize()
        {
            AssemblyPath = AssemblyPath.NormalizePath();
            FileNameConverters = FileNameConverters.Map(FileSystemExtensions.NormalizePath);
            TypeNameConverters = TypeNameConverters.Map(FileSystemExtensions.NormalizePath);
            PropertyNameConverters = PropertyNameConverters.Map(FileSystemExtensions.NormalizePath);
            EnumValueNameConverters = EnumValueNameConverters.Map(FileSystemExtensions.NormalizePath);
            return this;
        }

        public TgConfig MergeWithDefaultParams()
        {
            if (ExplicitPublicAccessor == null) ExplicitPublicAccessor = GeneratorOptions.DefaultExplicitPublicAccessor;
            if (TypeScriptFileExtension == null) TypeScriptFileExtension = GeneratorOptions.DefaultTypeScriptFileExtension;
            if (TabLength == null) TabLength = GeneratorOptions.DefaultTabLength;
            if (FileNameConverters == null) FileNameConverters = GetConverterNames(GeneratorOptions.DefaultFileNameConverters);
            if (TypeNameConverters == null) TypeNameConverters = GetConverterNames(GeneratorOptions.DefaultTypeNameConverters);
            if (PropertyNameConverters == null) PropertyNameConverters = GetConverterNames(GeneratorOptions.DefaultPropertyNameConverters);
            if (EnumValueNameConverters == null) EnumValueNameConverters = GetConverterNames(GeneratorOptions.DefaultEnumValueNameConverters);
            return this;
        }

        private string[] GetConverterNames(IEnumerable<IConverter> converters)
        {
            return converters
                .Select(c => c.GetType().Name)
                .ToArray();
        }

        [DataMember(Name = "assemblyPath")]
        public string AssemblyPath { get; set; }

        [DataMember(Name = "fileNameConverters")]
        public string[] FileNameConverters { get; set; }

        [DataMember(Name = "typeNameConverters")]
        public string[] TypeNameConverters { get; set; }

        [DataMember(Name = "propertyNameConverters")]
        public string[] PropertyNameConverters { get; set; }

        [DataMember(Name = "enumValueNameConverters")]
        public string[] EnumValueNameConverters { get; set; }

        [DataMember(Name = "typeScriptFileExtension")]
        public string TypeScriptFileExtension { get; set; }

        [DataMember(Name = "tabLength")]
        public int? TabLength { get; set; }

        [DataMember(Name = "explicitPublicAccessor")]
        public bool? ExplicitPublicAccessor { get; set; }
    }
}
