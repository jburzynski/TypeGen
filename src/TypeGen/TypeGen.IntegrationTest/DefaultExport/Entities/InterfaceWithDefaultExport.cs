using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.DefaultExport.Entities
{
    [ExportTsInterface(OutputDir = "default-export/")]
    [TsDefaultExport]
    public class InterfaceWithDefaultExport
    {
        
    }
}