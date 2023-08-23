using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.Blacklist.Entities;

[ExportTsInterface]
public interface InterfaceWithBlacklistedBase : IFoo
{
    
}