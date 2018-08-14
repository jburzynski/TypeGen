using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsClass]
    public class GenericWithRestrictions<T> where T: TestInterface
    {
        
    }
}