using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsEnum]
    public enum EnumShortValues : short
    {
        A = 1,
        B = 2
    }
}