using TypeGen.Core.TypeAnnotations;
using TypeGen.IntegrationTest.CommonCases.Entities.Structs;

namespace TypeGen.IntegrationTest.StructImplementsInterfaces.Entities;

[ExportTsClass]
public struct ImplementsInterfaces : IFoo, IBar
{
    
}