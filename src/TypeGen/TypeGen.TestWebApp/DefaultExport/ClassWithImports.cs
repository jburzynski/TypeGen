using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.DefaultExport
{
    [ExportTsClass(OutputDir = "default-export/")]
    [TsCustomBase("BaseWithDefaultExport", "./my-path/example/base", isDefaultExport: true)]
    public class ClassWithImports
    {
        public ClassWithDefaultExport ClassWithDefaultExport { get; set; }
        public ClassWithoutDefaultExport ClassWithoutDefaultExport { get; set; }
        public InterfaceWithDefaultExport InterfaceWithDefaultExport { get; set; }
        
        [TsType("TypeWithDefaultExport", "./my-path/example", isDefaultExport: true)]
        public string CustomTypeWithDefaultExport { get; set; }
    }
}