using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsInterface(OutputDir = "test-interfaces")]
    public class TestInterface
    {
        public string Property { get; set; }
    }
}
