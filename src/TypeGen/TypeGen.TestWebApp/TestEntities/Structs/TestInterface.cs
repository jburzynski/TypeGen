using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities.Structs
{
    [ExportTsInterface(OutputDir = "test-interfaces")]
    public struct TestInterface
    {
        public string Property { get; set; }
    }
}
