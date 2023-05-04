using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities.Structs
{
    [ExportTsInterface]
    public struct ReadonlyInterface
    {
        public readonly int ReadonlyField;
    }
}