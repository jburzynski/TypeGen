using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities.Structs
{
    [ExportTsInterface]
    public struct ReadonlyInterface
    {
        public readonly int ReadonlyField;
    }
}