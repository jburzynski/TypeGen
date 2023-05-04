using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities.Structs;

[ExportTsInterface]
[TsCustomBase("MB", "./my/base/my-base", "MyBase")]
public struct CustomBaseCustomImport
{
}
