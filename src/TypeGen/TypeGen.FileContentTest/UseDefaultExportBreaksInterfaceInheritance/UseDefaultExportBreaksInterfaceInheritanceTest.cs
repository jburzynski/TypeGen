using System;
using System.Threading.Tasks;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.TestingUtils;
using TypeGen.FileContentTest.UseDefaultExportBreaksInterfaceInheritance.Entities;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.FileContentTest.UseDefaultExportBreaksInterfaceInheritance;

public class UseDefaultExportBreaksInterfaceInheritanceTest : GenerationTestBase
{
    public UseDefaultExportBreaksInterfaceInheritanceTest(ITestOutputHelper output) : base(output) { }

    [Theory]
    [InlineData(typeof(ProductDto), "TypeGen.FileContentTest.UseDefaultExportBreaksInterfaceInheritance.Expected.product-dto.ts")]
    public async Task TestUseDefaultExportBreaksInterfaceInheritanceGenerationSpec(Type type, string expectedLocation)
    {
        var generationSpec = new UseDefaultExportBreaksInterfaceInheritanceGenerationSpec();
        var generatorOptions = new Core.Generator.GeneratorOptions
        {
            BaseOutputDirectory = "./types",
            UseDefaultExport = true
        };
        await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
    }
    
    private class UseDefaultExportBreaksInterfaceInheritanceGenerationSpec : GenerationSpec
    {
        public UseDefaultExportBreaksInterfaceInheritanceGenerationSpec()
        {
            AddClass<ProductDto>();
        }
    }
}