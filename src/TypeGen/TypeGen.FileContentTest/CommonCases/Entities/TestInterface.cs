using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsInterface(OutputDir = "test-interfaces")]
    public class TestInterface
    {
        public string Property { get; set; }
    }
}
