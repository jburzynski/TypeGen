using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsClass(OutputDir = "no/slash/output/dir")]
    public class NoSlashOutputDir
    {
        public GenericClass<string> Property { get; set; }
    }
}