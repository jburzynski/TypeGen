using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsClass]
    [TsIgnoreBase]
    public class WithIgnoredBase : NotGeneratedBaseClass
    {
        public string SomeProperty { get; set; }
    }
}
