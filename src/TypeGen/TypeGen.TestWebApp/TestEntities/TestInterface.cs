using TypeGen.Core.TypeAnnotations;

namespace CoreWebApp.TestEntities
{
    [ExportTsInterface(OutputDir = "test-interfaces")]
    public class TestInterface
    {
        public string Property { get; set; }
    }
}
