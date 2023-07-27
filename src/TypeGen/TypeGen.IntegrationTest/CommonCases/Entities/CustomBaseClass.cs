using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsClass]
    [TsCustomBase("AcmeCustomBase<string>")]
    public class CustomBaseClass
    {
        public string SomeProperty { get; set; }
    }
}
