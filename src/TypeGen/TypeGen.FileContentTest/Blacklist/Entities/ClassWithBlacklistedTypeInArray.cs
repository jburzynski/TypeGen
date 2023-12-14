using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.Blacklist.Entities;

[ExportTsClass]
public class ClassWithBlacklistedTypeInArray
{
    public Baz[] BazProp { get; set; }
}