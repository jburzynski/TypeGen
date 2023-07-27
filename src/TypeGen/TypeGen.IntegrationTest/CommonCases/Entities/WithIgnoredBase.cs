using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsClass]
    [TsIgnoreBase]
    public class WithIgnoredBase : NotGeneratedBaseClass
    {
        public string SomeProperty { get; set; }
    }
}
