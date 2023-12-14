using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities.Structs
{
    [ExportTsInterface]
    public struct ReadonlyInterface
    {
        public readonly int ReadonlyField;
    }
}