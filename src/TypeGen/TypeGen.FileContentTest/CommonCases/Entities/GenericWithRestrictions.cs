using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsClass]
    public class GenericWithRestrictions<T> where T: TestInterface
    {
        
    }
}