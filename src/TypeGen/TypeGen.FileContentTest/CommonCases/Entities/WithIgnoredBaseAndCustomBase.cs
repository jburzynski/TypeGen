using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsClass]
    [TsIgnoreBase]
    [TsCustomBase("SomeOtherBase")]
    public class WithIgnoredBaseAndCustomBase : NotGeneratedBaseClass
    {
    }
}
