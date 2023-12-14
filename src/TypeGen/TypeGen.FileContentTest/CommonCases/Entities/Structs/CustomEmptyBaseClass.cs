using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities.Structs
{
    [ExportTsInterface]
    [TsCustomBase]
    public struct CustomEmptyBaseClass
    {
        public int SomeProperty { get; set; }
    }
}
