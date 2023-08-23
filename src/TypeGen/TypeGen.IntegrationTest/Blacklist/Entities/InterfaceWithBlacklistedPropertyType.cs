using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.Blacklist.Entities;

[ExportTsInterface]
public interface InterfaceWithBlacklistedPropertyType
{
    Baz BazProp { get; set; }
}