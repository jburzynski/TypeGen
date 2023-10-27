using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsInterface]
    [TsCustomBase("MB", "./my/base/my-base", "MyBase")]
    public class CustomBaseCustomImport : CustomBaseClass
    {
    }
}
