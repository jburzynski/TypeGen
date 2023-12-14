using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities.Structs
{
    [ExportTsClass]
    public struct StaticReadonly
    {
        public static string StaticProperty { get; set; }
        
        [TsNotStatic]
        public static string NotStaticProperty { get; set; }

        public static readonly int StaticReadonlyField = 2;
        
        [TsDefaultValue(null)]
        public static readonly int StaticReadonlyFieldNoValue = 2;
    }
}