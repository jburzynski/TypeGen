using System;
using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    internal class AssemblySpec
    {
        public IDictionary<string, RegexExportRule> ClassRegexExportRules { get; }
        public IDictionary<string, RegexExportRule> InterfaceRegexExportRules { get; }
        public IDictionary<string, RegexExportRule> EnumRegexExportRules { get; }

        public AssemblySpec()
        {
            ClassRegexExportRules = new Dictionary<string, RegexExportRule>();
            InterfaceRegexExportRules = new Dictionary<string, RegexExportRule>();
            EnumRegexExportRules = new Dictionary<string, RegexExportRule>();
        }

        public void AddClassRegexExportRule(string regex, ExportAttribute exportAttribute, params Attribute[] additionalAttributes)
        {
            ClassRegexExportRules[regex] = new RegexExportRule(exportAttribute, additionalAttributes);
        }
        
        public void AddInterfaceRegexExportRule(string regex, ExportAttribute exportAttribute, params Attribute[] additionalAttributes)
        {
            InterfaceRegexExportRules[regex] = new RegexExportRule(exportAttribute, additionalAttributes);
        }
        
        public void AddEnumRegexExportRule(string regex, ExportAttribute exportAttribute, params Attribute[] additionalAttributes)
        {
            EnumRegexExportRules[regex] = new RegexExportRule(exportAttribute, additionalAttributes);
        }
    }
}