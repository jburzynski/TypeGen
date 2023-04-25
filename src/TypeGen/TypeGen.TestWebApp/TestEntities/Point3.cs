using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsStruct]
    public struct Point3
    {
        public int X;
        public int Y;
        public int Z;
    }
}