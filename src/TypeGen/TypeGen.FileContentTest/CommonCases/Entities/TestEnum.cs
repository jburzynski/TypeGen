using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsEnum(OutputDir = "test-enums", IsConst = true)]
    public enum TestEnum
    {
        A
    }
}