using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.DefaultExport.Entities
{
    [ExportTsClass(OutputDir = "default-export")]
    [TsDefaultExport]
    public class GenericClassWithDefaultExport<T1, T2>
    {
        
    }
}