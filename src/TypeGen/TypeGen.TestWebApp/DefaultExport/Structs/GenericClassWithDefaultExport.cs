using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.DefaultExport.Structs
{
    [ExportTsClass(OutputDir = "default-export")]
    [TsDefaultExport]
    public struct GenericClassWithDefaultExport<T1, T2>
    {
        
    }
}