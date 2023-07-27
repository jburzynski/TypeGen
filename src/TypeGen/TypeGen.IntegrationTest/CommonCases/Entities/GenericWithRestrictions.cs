using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsClass]
    public class GenericWithRestrictions<T> where T: TestInterface
    {
        
    }
}