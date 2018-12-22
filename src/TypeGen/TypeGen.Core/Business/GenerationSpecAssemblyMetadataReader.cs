using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Business
{
    internal class GenerationSpecAssemblyMetadataReader : IMetadataReader
    {
        private readonly GenerationSpec _spec;
        private readonly IMetadataReader _attributeMetadataReader;

        public GenerationSpecAssemblyMetadataReader(GenerationSpec spec, IMetadataReader attributeMetadataReader)
        {
            _spec = spec;
            _attributeMetadataReader = attributeMetadataReader;
        }

        private TAttribute GetAttributeFromRegexExportRule<TAttribute>(RegexExportRule rule) where TAttribute : Attribute
        {
            if (rule.ExportAttribute is TAttribute attribute) return attribute;
            return rule.AdditionalAttributes.FirstOrDefault(a => a is TAttribute) as TAttribute;
        }

        public TAttribute GetAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            Requires.NotNull(type, nameof(type));

            if (type.FullName == null) return null;

            AssemblySpec assemblySpec = _spec.AssemblySpecs[type.GetTypeInfo().Assembly];

            if (type.GetTypeInfo().IsClass)
            {
                RegexExportRule rule = assemblySpec.ClassRegexExportRules.FirstOrDefault(kvp => Regex.IsMatch(type.FullName, kvp.Key)).Value;
                if (rule != null) return GetAttributeFromRegexExportRule<TAttribute>(rule);
                
                rule = assemblySpec.InterfaceRegexExportRules.FirstOrDefault(kvp => Regex.IsMatch(type.FullName, kvp.Key)).Value;
                if (rule != null) return GetAttributeFromRegexExportRule<TAttribute>(rule);
            }
            else if (type.GetTypeInfo().IsEnum)
            {
                RegexExportRule rule = assemblySpec.EnumRegexExportRules.FirstOrDefault(kvp => Regex.IsMatch(type.FullName, kvp.Key)).Value;
                if (rule != null) return GetAttributeFromRegexExportRule<TAttribute>(rule);
            }

            return null;
        }

        public TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            return _attributeMetadataReader.GetAttribute<TAttribute>(memberInfo);
        }

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            return _attributeMetadataReader.GetAttributes<TAttribute>(memberInfo);
        }
    }
}