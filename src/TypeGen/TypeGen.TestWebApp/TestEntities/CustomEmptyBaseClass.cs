using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsInterface]
    [TsCustomBase]
    public class CustomEmptyBaseClass : BaseClass<string>
    {
        public int SomeProperty { get; set; }
    }
}
