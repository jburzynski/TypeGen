using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.ConstantsOnly.Entities
{
    [ExportTsClass]
    public class ConstantsOnly
    {
        public const string ConstString = "data";
        public const int ConstInt = 10;

        public static string StaticString = "data";
        public static int StaticInt = 10;

        public static readonly string StaticReadonlyString = "data";
        public static readonly int StaticReadonlyInt = 10;

        public string FieldString = "data";
        public int FieldInt = 10;

        public string PropString { get; set; } = "data";
        public int PropInt { get; set; } = 10;
    }
}