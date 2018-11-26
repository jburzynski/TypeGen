using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsClass]
    public class ExtendedPrimitivesClass
    {
        public Guid GuidProp { get; set; }
        public DateTimeOffset DateTimeOffsetProp { get; set; }
        public TimeSpan TimeSpanProp { get; set; }
    }
}