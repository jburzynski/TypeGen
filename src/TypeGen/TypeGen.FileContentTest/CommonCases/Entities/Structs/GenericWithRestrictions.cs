using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities.Structs
{
    [ExportTsClass]
    public struct GenericWithRestrictions<T> where T: Entities.TestInterface
    {
        
    }
}