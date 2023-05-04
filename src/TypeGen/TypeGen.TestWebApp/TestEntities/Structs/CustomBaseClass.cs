using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities.Structs
{
    [ExportTsClass]
    [TsCustomBase("AcmeCustomBase<string>")]
    public struct CustomBaseClass
    {
        public string SomeProperty { get; set; }
    }
}
