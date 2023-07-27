using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities;

[ExportTsEnum]
public enum EnumWithCustomHeadAndBody
{
    Foo = 0,
    Bar = 1
}