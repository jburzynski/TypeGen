using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsInterface(OutputDir = "./very/nested/directory/")]
    public class NestedEntity
    {
        public GenericClass<string> GenericClassProperty { get; set; }

        [TsOptional]
        public string OptionalProperty { get; set; }
    }
}
