using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsClass]
    public class StaticReadonly
    {
        public static string StaticProperty { get; set; }
        
        [TsNotStatic]
        public static string NotStaticProperty { get; set; }

        public static readonly int StaticReadonlyField;
        
        [TsDefaultValue(null)]
        public static readonly int StaticReadonlyFieldNoValue;
    }
}