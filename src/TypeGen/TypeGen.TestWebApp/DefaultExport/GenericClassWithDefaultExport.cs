using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.DefaultExport
{
    [ExportTsClass(OutputDir = "default-export")]
    [TsDefaultExport]
    public class GenericClassWithDefaultExport<T1, T2>
    {
        
    }
}