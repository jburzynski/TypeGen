using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Cli
{
    /// <summary>
    /// Represents console configuration parameters
    /// </summary>
    [DataContract]
    internal class ConfigParams
    {
        public ConfigParams Normalize()
        {
            AssemblyPath = AssemblyPath.NormalizePath();
            FileNameConverters = FileNameConverters.Map(StringExtensions.NormalizePath);
            TypeNameConverters = TypeNameConverters.Map(StringExtensions.NormalizePath);
            PropertyNameConverters = PropertyNameConverters.Map(StringExtensions.NormalizePath);
            EnumValueNameConverters = EnumValueNameConverters.Map(StringExtensions.NormalizePath);
            return this;
        }

        public ConfigParams MergeWithDefaultParams()
        {
            if (ExplicitPublicAccessor == null) ExplicitPublicAccessor = false;
            if (TypeScriptFileExtension == null) TypeScriptFileExtension = "ts";
            if (TabLength == null) TabLength = 4;
            if (FileNameConverters == null) FileNameConverters = new[] { "PascalCaseToKebabCase" };
            if (TypeNameConverters == null) TypeNameConverters = new string[0];
            if (PropertyNameConverters == null) PropertyNameConverters = new[] { "PascalCaseToCamelCase" };
            if (EnumValueNameConverters == null) EnumValueNameConverters = new string[0];
            return this;
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
