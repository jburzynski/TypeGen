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

        public AssemblySpecBuilder AddClasses(string typeNameRegex, string outputDir = null)
        {
            _activeFeature = ActiveFeature.ClassRegexExport;
            _activeRegex = typeNameRegex;
            
            _spec.AddClassRegexExportRule(typeNameRegex, new ExportTsClassAttribute { OutputDir = outputDir });

            return this;
        }
        
        public AssemblySpecBuilder AddInterfaces(string typeNameRegex, string outputDir = null)
        {
            _activeFeature = ActiveFeature.InterfaceRegexExport;
            _activeRegex = typeNameRegex;
            
            _spec.AddInterfaceRegexExportRule(typeNameRegex, new ExportTsInterfaceAttribute { OutputDir = outputDir });

            return this;
        }
        
        public AssemblySpecBuilder AddEnums(string typeNameRegex, string outputDir = null, bool isConst = false)
        {
            _activeFeature = ActiveFeature.EnumRegexExport;
            _activeRegex = typeNameRegex;
            
            _spec.AddEnumRegexExportRule(typeNameRegex, new ExportTsEnumAttribute { OutputDir = outputDir, IsConst = isConst });

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