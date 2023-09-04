using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities.Structs;

[ExportTsInterface]
[TsCustomBase("MB", "./my/base/my-base", "MyBase")]
public struct CustomBaseCustomImport
{
}
