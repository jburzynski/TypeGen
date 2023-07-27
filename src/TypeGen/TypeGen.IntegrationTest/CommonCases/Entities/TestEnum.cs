using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsEnum(OutputDir = "test-enums", IsConst = true)]
    public enum TestEnum
    {
        A
    }
}