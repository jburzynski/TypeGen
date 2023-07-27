using System;
using System.Threading.Tasks;
using TypeGen.Core.SpecGeneration;
using TypeGen.IntegrationTest.TestingUtils;
using TypeGen.IntegrationTest.UseDefaultExportBreaksInterfaceInheritance.Entities;
using Xunit;

namespace TypeGen.IntegrationTest.UseDefaultExportBreaksInterfaceInheritance;

public class UseDefaultExportBreaksInterfaceInheritanceTest : GenerationTestBase
{
    [Theory]
    [InlineData(typeof(ProductDto), "TypeGen.IntegrationTest.UseDefaultExportBreaksInterfaceInheritance.Expected.product-dto.ts")]
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