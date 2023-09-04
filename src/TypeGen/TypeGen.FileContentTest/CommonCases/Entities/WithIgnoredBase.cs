using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsClass]
    [TsIgnoreBase]
    public class WithIgnoredBase : NotGeneratedBaseClass
    {
        public string SomeProperty { get; set; }
    }
}
