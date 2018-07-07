using Core2WebApp.TestEntities;
using TypeGen.Core.TypeAnnotations;

namespace CoreWebApp.TestEntities
{
    [ExportTsClass]
    public class GenericClass<T> : GenericBaseClass<T>
    {
        public T GenericProperty { get; set; }
        public TestEnum EnumProperty { get; set; }
        public NestedEntity Nested { get; set; }
    }
}
