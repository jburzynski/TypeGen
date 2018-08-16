using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsEnum(OutputDir = "test-enums", IsConst = true)]
    public enum TestEnum
    {
        A
    }
}