using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities;

[ExportTsClass]
public class ArrayOfNullable
{
    public byte?[] Password { get; set; }
}