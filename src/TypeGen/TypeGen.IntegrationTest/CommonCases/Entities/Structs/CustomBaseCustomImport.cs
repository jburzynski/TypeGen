using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities.Structs;

[ExportTsInterface]
[TsCustomBase("MB", "./my/base/my-base", "MyBase")]
public struct CustomBaseCustomImport
{
}
