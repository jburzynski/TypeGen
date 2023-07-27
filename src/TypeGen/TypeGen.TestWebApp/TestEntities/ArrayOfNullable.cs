using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities;

[ExportTsClass]
public class ArrayOfNullable
{
    public byte?[] Password { get; set; }
}