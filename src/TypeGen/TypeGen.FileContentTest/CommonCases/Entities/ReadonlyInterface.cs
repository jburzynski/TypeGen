using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsInterface]
    public class ReadonlyInterface
    {
        public readonly int ReadonlyField;
    }
}