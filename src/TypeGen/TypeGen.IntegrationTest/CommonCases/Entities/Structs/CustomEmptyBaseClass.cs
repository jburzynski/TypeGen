using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities.Structs
{
    [ExportTsInterface]
    [TsCustomBase]
    public struct CustomEmptyBaseClass
    {
        public int SomeProperty { get; set; }
    }
}
