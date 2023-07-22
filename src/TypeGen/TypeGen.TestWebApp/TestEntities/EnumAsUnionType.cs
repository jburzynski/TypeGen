using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsEnum(AsUnionType = true)]
    public enum EnumAsUnionType
    {
        Zeta = 2,
        Ypsilon = 3,
        Delta = 4
    }
}