using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsClass]
    public class WithGenericBaseClassCustomType : BaseClass<TestClass<NestedEntity, BaseClass2<string>>>
    {
    }
}
