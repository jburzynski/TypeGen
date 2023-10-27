using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsClass(OutputDir = "no/slash/output/dir")]
    public class NoSlashOutputDir
    {
        public GenericClass<string> Property { get; set; }
    }
}