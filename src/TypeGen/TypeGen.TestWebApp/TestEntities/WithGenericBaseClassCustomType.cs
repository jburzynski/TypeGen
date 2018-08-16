using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsClass]
    public class WithGenericBaseClassCustomType : BaseClass<TestClass<NestedEntity, BaseClass2<string>>>
    {
    }
}
