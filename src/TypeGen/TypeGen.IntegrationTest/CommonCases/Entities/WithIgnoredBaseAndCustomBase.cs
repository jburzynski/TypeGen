using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsClass]
    [TsIgnoreBase]
    [TsCustomBase("SomeOtherBase")]
    public class WithIgnoredBaseAndCustomBase : NotGeneratedBaseClass
    {
    }
}
