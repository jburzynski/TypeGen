using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsClass]
    public class WithGenericBaseClassCustomType : BaseClass<TestClass<NestedEntity, BaseClass2<string>>>
    {
    }
}
