using System.Collections.Generic;

namespace TypeGen.Core.Generator.Services
{
    internal interface ITemplateService
    {
        string FillClassTemplate(string imports, string name, string extends, string implements, string properties, string constructor, string customHead, string customBody, string fileHeading = null);
        string FillClassDefaultExportTemplate(string imports, string name, string exportName, string extends, string implements, string properties, string constructor, string customHead, string customBody, string fileHeading = null);
        string FillConstructorTemplate(string type, string parameters, string superCall, string body);
        string FillConstructorAssignmentTemplate(string name);
        string FillClassPropertyTemplate(string modifiers, string name, string type, IEnumerable<string> typeUnions, bool isOptional, string defaultValue = null);
        string FillInterfaceTemplate(string imports, string name, string extends, string properties, string customHead, string customBody, string fileHeading = null);
        string FillInterfaceDefaultExportTemplate(string imports, string name, string exportName, string extends, string properties, string customHead, string customBody, string fileHeading = null);
        string FillInterfacePropertyTemplate(string modifiers, string name, string type, IEnumerable<string> typeUnions, bool isOptional);
        string FillEnumTemplate(string imports, string name, string values, bool isConst, bool asUnionType, string customHead, string customBody, string fileHeading = null);
        string FillEnumDefaultExportTemplate(string imports, string name, string values, bool isConst, bool asUnionType, string fileHeading = null);
        string FillEnumValueTemplate(string name, object value);
        string FillEnumUnionTypeValueTemplate(string name);
        string FillImportTemplate(string name, string typeAlias, string path);
        string FillImportDefaultExportTemplate(string name, string path);
        string FillIndexTemplate(string exports);
        string FillIndexExportTemplate(string filename);
        string GetExtendsText(string name);
        string GetExtendsText(IEnumerable<string> names);
        string GetImplementsText(IEnumerable<string> names);
    }
}