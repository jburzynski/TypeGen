using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsInterface]
    public interface ITestInterface
    {
        string StringProperty { get; }
        int IntProperty { get; set; }
        Guid GuidProperty { get; set; }
    }
}