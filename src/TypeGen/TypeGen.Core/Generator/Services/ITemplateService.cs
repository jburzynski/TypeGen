using System.Collections.Generic;

namespace TypeGen.Core.Generator.Services
{
    internal interface ITemplateService
    {
        string FillClassTemplate(string imports, string name, string extends, string implements, string properties, string customHead, string customBody, string fileHeading = null);
        string FillClassDefaultExportTemplate(string imports, string name, string exportName, string extends, string implements, string properties, string customHead, string customBody, string fileHeading = null);
        string FillClassPropertyTemplate(string modifiers, string name, string type, IEnumerable<string> typeUnions, string defaultValue = null);
        string FillInterfaceTemplate(string imports, string name, string extends, string properties, string customHead, string customBody, string fileHeading = null);
        string FillInterfaceDefaultExportTemplate(string imports, string name, string exportName, string extends, string properties, string customHead, string customBody, string fileHeading = null);
        string FillInterfacePropertyTemplate(string modifiers, string name, string type, IEnumerable<string> typeUnions, bool isOptional);
        string FillEnumTemplate(string imports, string name, string values, bool isConst, string fileHeading = null);
        string FillEnumDefaultExportTemplate(string imports, string name, string values, bool isConst, string fileHeading = null);
        string FillEnumValueTemplate(string name, object value);
        string FillImportTemplate(string name, string typeAlias, string path);
        string FillImportDefaultExportTemplate(string name, string path);
        string FillIndexTemplate(string exports);
        string FillIndexExportTemplate(string filename);
        string GetExtendsText(string name);
        string GetExtendsText(IEnumerable<string> name);
        string GetImplementsText(IEnumerable<string> names);
    }
}