using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities;

[ExportTsClass]
public class ArrayOfNullable
{
    public byte?[] Password { get; set; }
}