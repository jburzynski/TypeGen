using TypeGen.Core.TypeAnnotations;
using TypeGen.FileContentTest.CommonCases.Entities.Structs;

namespace TypeGen.FileContentTest.StructImplementsInterfaces.Entities;

[ExportTsClass]
public struct ImplementsInterfaces : IFoo, IBar
{
    
}