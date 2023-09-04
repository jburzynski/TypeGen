using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.Blacklist.Entities;

[ExportTsClass]
public class ClassWithBlacklistedPropertyType
{
    public Baz BazProp { get; set; }
}