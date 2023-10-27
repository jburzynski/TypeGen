using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsEnum]
    public enum EnumShortValues : short
    {
        A = 1,
        B = 2
    }
}