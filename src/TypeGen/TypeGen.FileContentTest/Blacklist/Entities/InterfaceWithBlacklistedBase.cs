using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.Blacklist.Entities;

[ExportTsInterface]
public interface InterfaceWithBlacklistedBase : IFoo
{
    
}