using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsClass]
    [TsIgnoreBase]
    [TsCustomBase("SomeOtherBase")]
    public class WithIgnoredBaseAndCustomBase : NotGeneratedBaseClass
    {
    }
}
