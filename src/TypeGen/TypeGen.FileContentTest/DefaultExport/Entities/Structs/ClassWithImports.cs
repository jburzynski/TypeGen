using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.DefaultExport.Entities.Structs
{
    [ExportTsClass(OutputDir = "default-export/")]
    [TsCustomBase("BaseWithDefaultExport", "./my-path/example/base", isDefaultExport: true)]
    public struct ClassWithImports
    {
        public ClassWithDefaultExport ClassWithDefaultExport { get; set; }
        public GenericClassWithDefaultExport<int, string> GenericClassWithDefaultExport { get; set; }
        public ClassWithoutDefaultExport ClassWithoutDefaultExport { get; set; }
        public InterfaceWithDefaultExport InterfaceWithDefaultExport { get; set; }
        
        [TsType("TypeWithDefaultExport", "./my-path/example", isDefaultExport: true)]
        public string CustomTypeWithDefaultExport { get; set; }
    }
}