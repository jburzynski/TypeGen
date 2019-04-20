using TypeGen.Core.Storage;
using TypeGen.Core.Utils;

namespace TypeGen.Core.Business
{
    /// <summary>
    /// Fills templates with data
    /// </summary>
    internal class TemplateService : ITemplateService
    {
        // dependencies

        private readonly IInternalStorage _internalStorage;

        private readonly string _enumTemplate;
        private readonly string _enumValueTemplate;
        private readonly string _classTemplate;
        private readonly string _classPropertyTemplate;
        private readonly string _interfaceTemplate;
        private readonly string _interfacePropertyTemplate;
        private readonly string _importTemplate;
        private readonly string _indexTemplate;
        private readonly string _indexExportTemplate;
        private readonly string _headingTemplate;

        public GeneratorOptions GeneratorOptions { get; set; }

        public TemplateService(IInternalStorage internalStorage)
        {
            _internalStorage = internalStorage;

            _enumTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Enum.tpl");
            _enumValueTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumValue.tpl");
            _classTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Class.tpl");
            _classPropertyTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassProperty.tpl");
            _interfaceTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Interface.tpl");
            _interfacePropertyTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceProperty.tpl");
            _importTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Import.tpl");
            _indexTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Index.tpl");
            _indexExportTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.IndexExport.tpl");
            _headingTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Heading.tpl");
        }

        public string FillClassTemplate(string imports, string name, string extends, string properties, string customHead, string customBody, string fileHeading = null)
        {
            if (fileHeading == null) fileHeading = _headingTemplate;
            
            return ReplaceSpecialChars(_classTemplate)
                .Replace(GetTag("imports"), imports)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("extends"), extends)
                .Replace(GetTag("properties"), properties)
                .Replace(GetTag("customHead"), customHead)
                .Replace(GetTag("customBody"), customBody)
                .Replace(GetTag("fileHeading"), fileHeading);
        }

        public string FillClassPropertyTemplate(string modifiers, string name, string type, string defaultValue = null)
        {
            type = string.IsNullOrWhiteSpace(type) ? "" : $": {type}";
            defaultValue = string.IsNullOrWhiteSpace(defaultValue) ? "" : $" = {defaultValue}";
            
            return ReplaceSpecialChars(_classPropertyTemplate)
                .Replace(GetTag("modifiers"), modifiers)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("type"), type)
                .Replace(GetTag("defaultValue"), defaultValue);
        }

        public string FillInterfaceTemplate(string imports, string name, string extends, string properties, string customHead, string customBody, string fileHeading = null)
        {
            if (fileHeading == null) fileHeading = _headingTemplate;
            
            return ReplaceSpecialChars(_interfaceTemplate)
                .Replace(GetTag("imports"), imports)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("extends"), extends)
                .Replace(GetTag("properties"), properties)
                .Replace(GetTag("customHead"), customHead)
                .Replace(GetTag("customBody"), customBody)
                .Replace(GetTag("fileHeading"), fileHeading);
        }

        public string FillInterfacePropertyTemplate(string modifiers, string name, string type, bool isOptional)
        {
            type = string.IsNullOrWhiteSpace(type) ? "" : $": {type}";
            
            return ReplaceSpecialChars(_interfacePropertyTemplate)
                .Replace(GetTag("modifiers"), modifiers)
                .Replace(GetTag("name"), name + (isOptional ? "?" : ""))
                .Replace(GetTag("type"), type);
        }

        public string FillEnumTemplate(string imports, string name, string values, bool isConst, string fileHeading = null)
        {
            if (fileHeading == null) fileHeading = _headingTemplate;
            
            return ReplaceSpecialChars(_enumTemplate)
                .Replace(GetTag("imports"), imports)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("values"), values)
                .Replace(GetTag("modifiers"), isConst ? " const" : "")
                .Replace(GetTag("fileHeading"), fileHeading);
        }

        public string FillEnumValueTemplate(string name, int? intValue = null, string stringValue = null)
        {
            string value = intValue != null ? intValue.ToString() : $@"""{stringValue}""";
            
            return ReplaceSpecialChars(_enumValueTemplate)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("value"), value);
        }

        public string FillImportTemplate(string name, string typeAlias, string path)
        {
            string aliasText = string.IsNullOrEmpty(typeAlias) ? "" : $" as {typeAlias}";

            return ReplaceSpecialChars(_importTemplate)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("aliasText"), aliasText)
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

        private string GetTag(string tagName) => $"$tg{{{tagName}}}";

        private string ReplaceSpecialChars(string template)
        {
            return template
                .Replace(GetTag("tab"), StringUtils.GetTabText(GeneratorOptions.TabLength))
                .Replace(GetTag("quot"), GeneratorOptions.SingleQuotes ? "'" : "\"");
        }
    }
}
