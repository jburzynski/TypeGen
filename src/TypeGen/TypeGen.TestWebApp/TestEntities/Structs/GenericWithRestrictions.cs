using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities.Structs
{
    [ExportTsClass]
    public struct GenericWithRestrictions<T> where T: TestEntities.TestInterface
    {
        
    }
}