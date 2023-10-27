using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities.Structs
{
    [ExportTsInterface]
    public struct ITestInterface
    {
        public string StringProperty { get; }
        public int IntProperty { get; set; }
        public Guid GuidProperty { get; set; }
    }
}