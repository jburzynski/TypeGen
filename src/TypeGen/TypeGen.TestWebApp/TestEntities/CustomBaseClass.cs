using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsClass]
    [TsCustomBase("AcmeCustomBase<string>")]
    public class CustomBaseClass
    {
        public string SomeProperty { get; set; }
    }
}
