using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities.Structs
{
    [ExportTsClass]
    public struct ExtendedPrimitivesClass
    {
        public Guid GuidProp { get; set; }
        public DateTime DateTimeProp { get; set; }
        public DateTimeOffset DateTimeOffsetProp { get; set; }

        public Guid GuidField = new Guid("8c78d9e8-5ade-4297-8160-7b49ae86a815");
        public DateTime DateTimeField = new DateTime(2010, 7, 15);
        public DateTimeOffset DateTimeOffsetField = new DateTimeOffset(new DateTime(2010, 7, 15), TimeSpan.FromHours(2));
        
        [TsType("string")]
        public DateTime DateTimeStringField = new DateTime(2010, 7, 15);
        [TsType("string")]
        public DateTimeOffset DateTimeOffsetStringField = new DateTimeOffset(new DateTime(2010, 7, 15), TimeSpan.FromHours(2));

        public ExtendedPrimitivesClass()
        {
            GuidProp = default;
            DateTimeProp = default;
            DateTimeOffsetProp = default;
        }
    }
}