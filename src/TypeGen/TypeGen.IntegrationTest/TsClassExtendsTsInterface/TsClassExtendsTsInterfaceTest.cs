using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.IntegrationTest.TestingUtils;
using TypeGen.IntegrationTest.TsClassExtendsTsInterface.Entities;
using Xunit;

namespace TypeGen.IntegrationTest.TsClassExtendsTsInterface;

public class TsClassExtendsTsInterfaceTest : GenerationTestBase
{
    [Fact]
    public async Task TsClassExtendsTsInterface_Test()
    {
        var type = typeof(TsClass);
        const string expectedLocation = "TypeGen.IntegrationTest.TsClassExtendsTsInterface.Expected.ts-class.ts";
        var generationSpec = new TsClassExtendsTsInterfaceGenerationSpec();
        var generatorOptions = new GeneratorOptions();

        await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
    }
    
    private class TsClassExtendsTsInterfaceGenerationSpec : GenerationSpec
    {
        public TsClassExtendsTsInterfaceGenerationSpec()
        {
            AddClass<TsClass>();
            AddInterface<TsInterface>();
        }
    }
}