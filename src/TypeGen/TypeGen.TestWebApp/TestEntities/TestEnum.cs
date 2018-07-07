using TypeGen.Core.TypeAnnotations;

namespace CoreWebApp.TestEntities
{
    [ExportTsEnum(OutputDir = "test-enums", IsConst = true)]
    public enum TestEnum
    {
        A
    }
}