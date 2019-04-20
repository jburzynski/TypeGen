namespace TypeGen.Core.Business
{
    internal interface ITemplateService
    {
        GeneratorOptions GeneratorOptions { get; set; }
        string FillClassTemplate(string imports, string name, string extends, string properties, string customHead, string customBody, string fileHeading = null);
        string FillClassPropertyTemplate(string modifiers, string name, string type, string defaultValue = null);
        string FillInterfaceTemplate(string imports, string name, string extends, string properties, string customHead, string customBody, string fileHeading = null);
        string FillInterfacePropertyTemplate(string modifiers, string name, string type, bool isOptional);
        string FillEnumTemplate(string imports, string name, string values, bool isConst, string fileHeading = null);
        string FillEnumValueTemplate(string name, int? intValue = null, string stringValue = null);
        string FillImportTemplate(string name, string typeAlias, string path);
        string FillIndexTemplate(string exports);
        string FillIndexExportTemplate(string filename);
        string GetExtendsText(string name);
    }
}