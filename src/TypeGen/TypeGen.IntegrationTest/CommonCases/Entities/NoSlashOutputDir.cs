using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsClass(OutputDir = "no/slash/output/dir")]
    public class NoSlashOutputDir
    {
        public GenericClass<string> Property { get; set; }
    }
}