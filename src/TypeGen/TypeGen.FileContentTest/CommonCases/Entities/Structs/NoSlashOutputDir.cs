using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities.Structs
{
    [ExportTsClass(OutputDir = "no/slash/output/dir")]
    public struct NoSlashOutputDir
    {
        public GenericClass<string> Property { get; set; }
    }
}