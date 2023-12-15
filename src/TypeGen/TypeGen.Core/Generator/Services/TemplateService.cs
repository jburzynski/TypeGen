using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TypeGen.Core.Storage;
using TypeGen.Core.Utils;

namespace TypeGen.Core.Generator.Services
{
    /// <summary>
    /// Fills templates with data
    /// </summary>
    internal class TemplateService : ITemplateService
    {
        // dependencies

        private readonly IInternalStorage _internalStorage;
        private readonly IGeneratorOptionsProvider _generatorOptionsProvider;

        private readonly string _enumTemplate;
        private readonly string _enumUnionTypeTemplate;
        private readonly string _enumDefaultExportTemplate;
        private readonly string _enumUnionTypeDefaultExportTemplate;
        private readonly string _enumValueTemplate;
        private readonly string _enumUnionTypeValueTemplate;
        private readonly string _classTemplate;
        private readonly string _classDefaultExportTemplate;
        private readonly string _constructorTemplate;
        private readonly string _constructorAssignmentTemplate;
        private readonly string _classPropertyTemplate;
        private readonly string _interfaceTemplate;
        private readonly string _interfaceDefaultExportTemplate;
        private readonly string _interfacePropertyTemplate;
        private readonly string _importTemplate;
        private readonly string _importDefaultExportTemplate;
        private readonly string _indexTemplate;
        private readonly string _indexExportTemplate;
        private readonly string _headingTemplate;

        private GeneratorOptions GeneratorOptions => _generatorOptionsProvider.GeneratorOptions;

        public TemplateService(IInternalStorage internalStorage, IGeneratorOptionsProvider generatorOptionsProvider)
        {
            _internalStorage = internalStorage;
            _generatorOptionsProvider = generatorOptionsProvider;

            _enumTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Enum.tpl");
            _enumUnionTypeTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumUnionType.tpl");
            _enumDefaultExportTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumDefaultExport.tpl");
            _enumUnionTypeDefaultExportTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumUnionTypeDefaultExport.tpl");
            _enumValueTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumValue.tpl");
            _enumUnionTypeValueTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumUnionTypeValue.tpl");
            _classTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Class.tpl");
            _classDefaultExportTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassDefaultExport.tpl");
            _constructorTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Constructor.tpl");
            _constructorAssignmentTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ConstructorAssignment.tpl");
            _classPropertyTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassProperty.tpl");
            _interfaceTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Interface.tpl");
            _interfaceDefaultExportTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceDefaultExport.tpl");
            _interfacePropertyTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceProperty.tpl");
            _importTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Import.tpl");
            _importDefaultExportTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ImportDefaultExport.tpl");
            _indexTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Index.tpl");
            _indexExportTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.IndexExport.tpl");
            _headingTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Heading.tpl");
        }

        public string FillClassTemplate(string imports, string name, string extends, string implements, string properties,
            string constructor, string tsDoc, string customHead, string customBody, string fileHeading = null)
        {
            if (fileHeading == null) fileHeading = _headingTemplate;
            
            return ReplaceSpecialChars(_classTemplate)
                .Replace(GetTag("imports"), imports)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("extends"), extends)
                .Replace(GetTag("implements"), implements)
                .Replace(GetTag("properties"), properties)
                .Replace(GetTag("constructor"), constructor)
                .Replace(GetTag("tsDoc"), tsDoc)
                .Replace(GetTag("customHead"), customHead)
                .Replace(GetTag("customBody"), customBody)
                .Replace(GetTag("fileHeading"), fileHeading);
        }
        
        public string FillClassDefaultExportTemplate(string imports, string name, string exportName, string extends, string implements,
            string properties, string constructor, string tsDoc, string customHead, string customBody, string fileHeading = null)
        {
            if (fileHeading == null) fileHeading = _headingTemplate;
            
            return ReplaceSpecialChars(_classDefaultExportTemplate)
                .Replace(GetTag("imports"), imports)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("exportName"), exportName)
                .Replace(GetTag("extends"), extends)
                .Replace(GetTag("implements"), implements)
                .Replace(GetTag("properties"), properties)
                .Replace(GetTag("constructor"), constructor)
                .Replace(GetTag("tsDoc"), tsDoc)
                .Replace(GetTag("customHead"), customHead)
                .Replace(GetTag("customBody"), customBody)
                .Replace(GetTag("fileHeading"), fileHeading);
        }

        public string FillConstructorTemplate(string type, string parameters, string superCall, string body)
        {
            return ReplaceSpecialChars(_constructorTemplate)
                .Replace(GetTag("type"), type)
                .Replace(GetTag("parameters"), parameters)
                .Replace(GetTag("super"), superCall)
                .Replace(GetTag("body"), body);
        }

        public string FillConstructorAssignmentTemplate(string name)
        {
            return ReplaceSpecialChars(_constructorAssignmentTemplate)
                .Replace(GetTag("name"), name);
        }

        public string FillClassPropertyTemplate(string modifiers, string name, string type, IEnumerable<string> typeUnions,
            bool isOptional, string tsDoc, string defaultValue = null)
        {
            type = $": {type}";
            type = ConcatenateWithTypeUnions(type, typeUnions);
            
            defaultValue = string.IsNullOrWhiteSpace(defaultValue) ? "" : $" = {defaultValue}";
            
            return ReplaceSpecialChars(_classPropertyTemplate)
                .Replace(GetTag("modifiers"), modifiers)
                .Replace(GetTag("name"), name + (isOptional ? "?" : ""))
                .Replace(GetTag("type"), type)
                .Replace(GetTag("tsDoc"), tsDoc)
                .Replace(GetTag("defaultValue"), defaultValue);
        }

        public string FillInterfaceTemplate(string imports, string name, string extends, string properties, string tsDoc,
            string customHead,string customBody, string fileHeading = null)
        {
            if (fileHeading == null) fileHeading = _headingTemplate;
            
            return ReplaceSpecialChars(_interfaceTemplate)
                .Replace(GetTag("imports"), imports)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("extends"), extends)
                .Replace(GetTag("properties"), properties)
                .Replace(GetTag("tsDoc"), tsDoc)
                .Replace(GetTag("customHead"), customHead)
                .Replace(GetTag("customBody"), customBody)
                .Replace(GetTag("fileHeading"), fileHeading);
        }
        
        public string FillInterfaceDefaultExportTemplate(string imports, string name, string exportName, string extends, string properties,
            string tsDoc, string customHead, string customBody, string fileHeading = null)
        {
            if (fileHeading == null) fileHeading = _headingTemplate;
            
            return ReplaceSpecialChars(_interfaceDefaultExportTemplate)
                .Replace(GetTag("imports"), imports)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("exportName"), exportName)
                .Replace(GetTag("extends"), extends)
                .Replace(GetTag("properties"), properties)
                .Replace(GetTag("tsDoc"), tsDoc)
                .Replace(GetTag("customHead"), customHead)
                .Replace(GetTag("customBody"), customBody)
                .Replace(GetTag("fileHeading"), fileHeading);
        }

        public string FillInterfacePropertyTemplate(string modifiers, string name, string type, IEnumerable<string> typeUnions, bool isOptional,
            string tsDoc)
        {
            type = $": {type}";
            type = ConcatenateWithTypeUnions(type, typeUnions);
            
            return ReplaceSpecialChars(_interfacePropertyTemplate)
                .Replace(GetTag("modifiers"), modifiers)
                .Replace(GetTag("name"), name + (isOptional ? "?" : ""))
                .Replace(GetTag("type"), type)
                .Replace(GetTag("tsDoc"), tsDoc);
        }

        public string FillEnumTemplate(string imports, string name, string values, bool isConst, bool asUnionType, string tsDoc,
            string customHead, string customBody, string fileHeading = null)
        {
            if (fileHeading == null) fileHeading = _headingTemplate;
            
            return ReplaceSpecialChars(asUnionType ? _enumUnionTypeTemplate : _enumTemplate)
                .Replace(GetTag("imports"), imports)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("values"), values)
                .Replace(GetTag("tsDoc"), tsDoc)
                .Replace(GetTag("customHead"), customHead)
                .Replace(GetTag("customBody"), customBody)
                .Replace(GetTag("modifiers"), isConst ? "const " : "")
                .Replace(GetTag("fileHeading"), fileHeading);
        }
        
        public string FillEnumDefaultExportTemplate(string imports, string name, string values, string tsDoc,
            bool isConst, bool asUnionType, string fileHeading = null)
        {
            if (fileHeading == null) fileHeading = _headingTemplate;
            
            return ReplaceSpecialChars(asUnionType ? _enumUnionTypeDefaultExportTemplate : _enumDefaultExportTemplate)
                .Replace(GetTag("imports"), imports)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("values"), values)
                .Replace(GetTag("tsDoc"), tsDoc)
                .Replace(GetTag("modifiers"), isConst ? "const " : "")
                .Replace(GetTag("fileHeading"), fileHeading);
        }

        public string FillEnumValueTemplate(string name, object value, string tsDoc)
        {
            char quote = GeneratorOptions.SingleQuotes ? '\'' : '"';
            string valueString = value is string str ? $@"{quote}{str}{quote}" : value.ToString();
            
            return ReplaceSpecialChars(_enumValueTemplate)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("value"), valueString)
                .Replace(GetTag("tsDoc"), tsDoc);
        }

        public string FillEnumUnionTypeValueTemplate(string name)
        {
            char quote = GeneratorOptions.SingleQuotes ? '\'' : '"';

            return ReplaceSpecialChars(_enumUnionTypeValueTemplate)
                .Replace(GetTag("name"), $@"{quote}{name}{quote}");
        }

        public string FillImportTemplate(string name, string typeAlias, string path, bool useImportType)
        {
            string aliasText = string.IsNullOrEmpty(typeAlias) ? "" : $" as {typeAlias}";

            return ReplaceSpecialChars(_importTemplate)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("aliasText"), aliasText)
                .Replace(GetTag("importType"), useImportType ? " type" : "")
                .Replace(GetTag("path"), path);
        }
        
        public string FillImportDefaultExportTemplate(string name, string path, bool useImportType)
        {
            return ReplaceSpecialChars(_importDefaultExportTemplate)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("importType"), useImportType ? " type" : "")
                .Replace(GetTag("path"), path);
        }

        public string FillIndexTemplate(string exports)
        {
            return ReplaceSpecialChars(_indexTemplate)
                .Replace(GetTag("exports"), exports);
        }

        public string FillIndexExportTemplate(string filename)
        {
            return ReplaceSpecialChars(_indexExportTemplate)
                .Replace(GetTag("filename"), filename);
        }

        public string GetExtendsText(string name) => $" extends {name}";
        public string GetExtendsText(IEnumerable<string> names) => $" extends {string.Join(", ", names)}";

        public string GetImplementsText(IEnumerable<string> names) => $" implements {string.Join(", ", names)}";

        private static string GetTag(string tagName) => $"$tg{{{tagName}}}";

        private string ReplaceSpecialChars(string template)
        {
            string tab = GeneratorOptions.UseTabCharacter ? "\t" : StringUtils.GetTabText(GeneratorOptions.TabLength);
            
            return template
                .Replace(GetTag("tab"), tab)
                .Replace(GetTag("quot"), GeneratorOptions.SingleQuotes ? "'" : "\"");
        }

        private static string ConcatenateWithTypeUnions(string text, IEnumerable<string> typeUnions)
        {
            if (typeUnions?.Any() == false) return text;
            
            string typeUnionsText = string.Join(" | ", typeUnions);
            return text + " | " + typeUnionsText;
        }
    }
}
