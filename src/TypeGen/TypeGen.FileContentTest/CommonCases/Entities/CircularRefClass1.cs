using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    public class CircularRefClass1
    {
        public CircularRefClass2 CircularRefClass2 { get; set; }

        [TsType(TsType.String)]
        public Guid GuidProperty { get; set; }
    }
}
