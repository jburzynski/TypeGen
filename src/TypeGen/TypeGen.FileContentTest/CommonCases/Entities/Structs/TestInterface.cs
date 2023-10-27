using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities.Structs
{
    [ExportTsInterface(OutputDir = "test-interfaces")]
    public struct TestInterface
    {
        public string Property { get; set; }
    }
}
