using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities;

[ExportTsEnum]
public enum EnumWithCustomHeadAndBody
{
    Foo = 0,
    Bar = 1
}