using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsInterface]
    public class ReadonlyInterface
    {
        public readonly int ReadonlyField;
    }
}