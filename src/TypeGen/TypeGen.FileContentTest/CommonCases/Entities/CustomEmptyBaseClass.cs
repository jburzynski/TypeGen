using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsInterface]
    [TsCustomBase]
    public class CustomEmptyBaseClass : BaseClass<string>
    {
        public int SomeProperty { get; set; }
    }
}
