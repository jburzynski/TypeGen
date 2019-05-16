using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsClass]
    public class DefaultMemberValues
    {
        public string fieldString = "fieldString";
        public static int staticFieldNumber = 2;

        public int PropertyNumber { get; set; } = 3;
        public static string StaticPropertyString { get; set; } = "StaticPropertyString";
    }
}