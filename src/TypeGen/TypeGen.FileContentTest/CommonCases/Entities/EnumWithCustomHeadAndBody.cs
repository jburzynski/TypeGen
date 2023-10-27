using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities;

[ExportTsEnum]
public enum EnumWithCustomHeadAndBody
{
    Foo = 0,
    Bar = 1
}