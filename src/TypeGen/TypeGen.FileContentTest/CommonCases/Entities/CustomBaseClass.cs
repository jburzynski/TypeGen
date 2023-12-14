using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsClass]
    [TsCustomBase("AcmeCustomBase<string>")]
    public class CustomBaseClass
    {
        public string SomeProperty { get; set; }
    }
}
