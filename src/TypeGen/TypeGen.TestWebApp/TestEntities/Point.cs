using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsStruct]
    public struct Point
    {
        [TsDefaultValue("0")] public int X;
        [TsDefaultValue("0")] public int Y;
    }
}