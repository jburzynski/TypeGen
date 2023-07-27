using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsInterface(OutputDir = "test-interfaces")]
    public class TestInterface
    {
        public string Property { get; set; }
    }
}
