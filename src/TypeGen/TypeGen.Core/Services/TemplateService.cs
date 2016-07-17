using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core.Services
{
    /// <summary>
    /// Contains logic for filling templates with data
    /// </summary>
    internal class TemplateService
    {
        private string _enumTemplate;
        private string _enumValueTemplate;
        private string _classTemplate;
        private string _classPropertyTemplate;
        private string _classPropertyWithDefaultValueTemplate;
        private string _interfaceTemplate;
        private string _interfacePropertyTemplate;
        private string _importTemplate;

        private readonly int _tabLength;

        public TemplateService(int tabLength)
        {
            _tabLength = tabLength;
        }

        public void Initialize()
        {
            _enumTemplate = Utilities.GetEmbeddedResource("TypeGen.Core.Templates.Enum.tpl");
            _enumValueTemplate = Utilities.GetEmbeddedResource("TypeGen.Core.Templates.EnumValue.tpl");
            _classTemplate = Utilities.GetEmbeddedResource("TypeGen.Core.Templates.Class.tpl");
            _classPropertyTemplate = Utilities.GetEmbeddedResource("TypeGen.Core.Templates.ClassProperty.tpl");
            _classPropertyWithDefaultValueTemplate = Utilities.GetEmbeddedResource("TypeGen.Core.Templates.ClassPropertyWithDefaultValue.tpl");
            _interfaceTemplate = Utilities.GetEmbeddedResource("TypeGen.Core.Templates.Interface.tpl");
            _interfacePropertyTemplate = Utilities.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceProperty.tpl");
            _importTemplate = Utilities.GetEmbeddedResource("TypeGen.Core.Templates.Import.tpl");
        }

        public string FillClassTemplate(string imports, string name, string properties)
        {
            return ReplaceTabs(_classTemplate)
                .Replace("$tg{imports}", imports)
                .Replace("$tg{name}", name)
                .Replace("$tg{properties}", properties);
        }

        public string FillClassPropertyWithDefaultValueTemplate(string accessor, string name, string defaultValue)
        {
            return ReplaceTabs(_classPropertyWithDefaultValueTemplate)
                .Replace("$tg{accessor}", accessor)
                .Replace("$tg{name}", name)
                .Replace("$tg{defaultValue}", defaultValue);
        }

        public string FillClassPropertyTemplate(string accessor, string name, string type)
        {
            return ReplaceTabs(_classPropertyTemplate)
                .Replace("$tg{accessor}", accessor)
                .Replace("$tg{name}", name)
                .Replace("$tg{type}", type);
        }

        public string FillInterfaceTemplate(string imports, string name, string properties)
        {
            return ReplaceTabs(_interfaceTemplate)
                .Replace("$tg{imports}", imports)
                .Replace("$tg{name}", name)
                .Replace("$tg{properties}", properties);
        }

        public string FillInterfacePropertyTemplate(string name, string type)
        {
            return ReplaceTabs(_interfacePropertyTemplate)
                .Replace("$tg{name}", name)
                .Replace("$tg{type}", type);
        }

        public string FillEnumTemplate(string imports, string name, string values)
        {
            return ReplaceTabs(_enumTemplate)
                .Replace("$tg{imports}", imports)
                .Replace("$tg{name}", name)
                .Replace("$tg{values}", values);
        }

        public string FillEnumValueTemplate(string name, int intValue)
        {
            return ReplaceTabs(_enumValueTemplate)
                .Replace("$tg{name}", name)
                .Replace("$tg{number}", intValue.ToString());
        }

        public string FillImportTemplate(string name, string path)
        {
            return ReplaceTabs(_importTemplate)
                .Replace("$tg{name}", name)
                .Replace("$tg{path}", path);
        }

        private string ReplaceTabs(string template)
        {
            return template.Replace("$tg{tab}", Utilities.GetTabText(_tabLength));
        }
    }
}
