using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities.Structs
{
    [ExportTsClass]
    public struct StrictNullsClass
    {
        public int SomeInt1 { get; set; }

        public int? SomeInt2 { get; set; }

        [TsNull]
        public int SomeInt3 { get; set; }

        [TsUndefined]
        public int SomeInt4 { get; set; }

        [TsNotUndefined]
        public int SomeInt5 { get; set; }

        [TsNotNull]
        public int SomeInt6 { get; set; }

        [TsNull, TsUndefined]
        public int SomeInt7 { get; set; }

        [TsNull, TsUndefined]
        public int? SomeInt8 { get; set; }

        [TsNotNull]
        public int? SomeInt9 { get; set; }

        [TsNotUndefined]
        public int? SomeInt10 { get; set; }

        [TsNotNull, TsNotUndefined]
        public int? SomeInt11 { get; set; }
    }
}
