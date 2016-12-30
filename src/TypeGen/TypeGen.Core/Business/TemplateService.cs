using TypeGen.Core.Storage;
using TypeGen.Core.Utils;

namespace TypeGen.Core.Business
{
    /// <summary>
    /// Contains logic for filling templates with data
    /// </summary>
    internal class TemplateService
    {
        // dependencies

        private readonly InternalStorage _internalStorage;

        private string _enumTemplate;
        private string _enumValueTemplate;
        private string _classTemplate;
        private string _classPropertyTemplate;
        private string _classPropertyWithDefaultValueTemplate;
        private string _interfaceTemplate;
        private string _interfacePropertyTemplate;
        private string _importTemplate;

        public int TabLength { get; set; }
        public bool SingleQuotes { get; set; }

        public TemplateService(InternalStorage internalStorage)
        {
            _internalStorage = internalStorage;
            LoadTemplates();
        }

        private void LoadTemplates()
        {
            _enumTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Enum.tpl");
            _enumValueTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumValue.tpl");
            _classTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Class.tpl");
            _classPropertyTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassProperty.tpl");
            _classPropertyWithDefaultValueTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassPropertyWithDefaultValue.tpl");
            _interfaceTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Interface.tpl");
            _interfacePropertyTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceProperty.tpl");
            _importTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Import.tpl");
        }

        public string FillClassTemplate(string imports, string name, string extends, string properties, string customHead, string customBody)
        {
            return ReplaceSpecialChars(_classTemplate)
                .Replace("$tg{imports}", imports)
                .Replace("$tg{name}", name)
                .Replace("$tg{extends}", extends)
                .Replace("$tg{properties}", properties)
                .Replace("$tg{customHead}", customHead)
                .Replace("$tg{customBody}", customBody);
        }

        public string FillClassPropertyWithDefaultValueTemplate(string accessor, string name, string defaultValue)
        {
            return ReplaceSpecialChars(_classPropertyWithDefaultValueTemplate)
                .Replace("$tg{accessor}", accessor)
                .Replace("$tg{name}", name)
                .Replace("$tg{defaultValue}", defaultValue);
        }

        public string FillClassPropertyTemplate(string accessor, string name, string type)
        {
            return ReplaceSpecialChars(_classPropertyTemplate)
                .Replace("$tg{accessor}", accessor)
                .Replace("$tg{name}", name)
                .Replace("$tg{type}", type);
        }

        public string FillInterfaceTemplate(string imports, string name, string extends, string properties, string customHead, string customBody)
        {
            return ReplaceSpecialChars(_interfaceTemplate)
                .Replace("$tg{imports}", imports)
                .Replace("$tg{name}", name)
                .Replace("$tg{extends}", extends)
                .Replace("$tg{properties}", properties)
                .Replace("$tg{customHead}", customHead)
                .Replace("$tg{customBody}", customBody);
        }

        public string FillInterfacePropertyTemplate(string name, string type)
        {
            return ReplaceSpecialChars(_interfacePropertyTemplate)
                .Replace("$tg{name}", name)
                .Replace("$tg{type}", type);
        }

        public string FillEnumTemplate(string imports, string name, string values)
        {
            return ReplaceSpecialChars(_enumTemplate)
                .Replace("$tg{imports}", imports)
                .Replace("$tg{name}", name)
                .Replace("$tg{values}", values);
        }

        public string FillEnumValueTemplate(string name, int intValue)
        {
            return ReplaceSpecialChars(_enumValueTemplate)
                .Replace("$tg{name}", name)
                .Replace("$tg{number}", intValue.ToString());
        }

        public string FillImportTemplate(string name, string asAlias, string path)
        {
            return ReplaceSpecialChars(_importTemplate)
                .Replace("$tg{name}", name)
                .Replace("$tg{asAlias}", asAlias)
                .Replace("$tg{path}", path);
        }

        private string ReplaceSpecialChars(string template)
        {
            return template
                .Replace("$tg{tab}", StringUtils.GetTabText(TabLength))
                .Replace("$tg{quot}", SingleQuotes ? "'" : "\"");
        }
    }
}
