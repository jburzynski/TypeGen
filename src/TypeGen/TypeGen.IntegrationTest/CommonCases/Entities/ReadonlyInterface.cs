using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsInterface]
    public class ReadonlyInterface
    {
        public readonly int ReadonlyField;
    }
}