using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    public class AssemblySpecBuilder
    {
        private readonly AssemblySpec _spec;
        private string _activeRegex;
        private ActiveFeature _activeFeature;

        private enum ActiveFeature
        {
            ClassRegexExport,
            InterfaceRegexExport,
            EnumRegexExport
        }
        
        internal AssemblySpecBuilder(AssemblySpec spec)
        {
            _spec = spec;
        }

        public AssemblySpecBuilder AddClasses(string regex, string outputDir = null)
        {
            _activeFeature = ActiveFeature.ClassRegexExport;
            _activeRegex = regex;
            
            _spec.AddClassRegexExportRule(regex, new ExportTsClassAttribute { OutputDir = outputDir });

            return this;
        }
        
        public AssemblySpecBuilder AddInterfaces(string regex, string outputDir = null)
        {
            _activeFeature = ActiveFeature.InterfaceRegexExport;
            _activeRegex = regex;
            
            _spec.AddInterfaceRegexExportRule(regex, new ExportTsInterfaceAttribute { OutputDir = outputDir });

            return this;
        }
        
        public AssemblySpecBuilder AddEnums(string regex, string outputDir = null, bool isConst = false)
        {
            _activeFeature = ActiveFeature.EnumRegexExport;
            _activeRegex = regex;
            
            _spec.AddEnumRegexExportRule(regex, new ExportTsEnumAttribute { OutputDir = outputDir, IsConst = isConst });

            return this;
        }

        public AssemblySpecBuilder CustomBase(string @base = null, string importType = null, string originalTypeName = null)
        {
            IDictionary<string, RegexExportRule> rules = GetActiveRegexExportRules();
            rules[_activeRegex].AddAdditionalAttribute(new TsCustomBaseAttribute(@base, importType, originalTypeName));

            return this;
        }

        public AssemblySpecBuilder IgnoreBase()
        {
            IDictionary<string, RegexExportRule> rules = GetActiveRegexExportRules();
            rules[_activeRegex].AddAdditionalAttribute(new TsIgnoreBaseAttribute());

            return this;
        }

        private IDictionary<string, RegexExportRule> GetActiveRegexExportRules()
        {
            switch (_activeFeature)
            {
                case ActiveFeature.ClassRegexExport:
                    return _spec.ClassRegexExportRules;
                case ActiveFeature.InterfaceRegexExport:
                    return _spec.InterfaceRegexExportRules;
                case ActiveFeature.EnumRegexExport:
                    return _spec.EnumRegexExportRules;
                default:
                    throw new CoreException("Wrong method invocation order: additional rules need to be specified after adding a regex rule");
            }
        }
    }
}