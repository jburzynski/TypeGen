using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsInterface(OutputDir = "./very/nested/directory/")]
    public class NestedEntity
    {
        public GenericClass<string> GenericClassProperty { get; set; }

        [TsOptional]
        public string OptionalProperty { get; set; }
    }
}
