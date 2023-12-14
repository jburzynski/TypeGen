using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsInterface]
    public interface ITestInterface
    {
        string StringProperty { get; }
        int IntProperty { get; set; }
        Guid GuidProperty { get; set; }
    }
}