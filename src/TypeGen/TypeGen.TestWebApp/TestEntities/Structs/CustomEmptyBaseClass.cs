using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities.Structs
{
    [ExportTsInterface]
    [TsCustomBase]
    public struct CustomEmptyBaseClass
    {
        public int SomeProperty { get; set; }
    }
}
