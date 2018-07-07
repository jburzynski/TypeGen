using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsInterface]
    [TsCustomBase("MB", "./my/base/my-base", "MyBase")]
    public class CustomBaseCustomImport : CustomBaseClass
    {
    }
}
