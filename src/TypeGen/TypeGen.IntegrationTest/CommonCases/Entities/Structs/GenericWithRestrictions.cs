using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities.Structs
{
    [ExportTsClass]
    public struct GenericWithRestrictions<T> where T: Entities.TestInterface
    {
        
    }
}