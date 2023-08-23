using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.Blacklist.Entities;

[ExportTsClass]
public class ClassWithBlacklistedPropertyType
{
    public Baz BazProp { get; set; }
}