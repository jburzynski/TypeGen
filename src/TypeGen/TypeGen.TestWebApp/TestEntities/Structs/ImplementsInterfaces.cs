using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities.Structs;

[ExportTsClass]
public struct ImplementsInterfaces : IFoo, IBar
{
    
}