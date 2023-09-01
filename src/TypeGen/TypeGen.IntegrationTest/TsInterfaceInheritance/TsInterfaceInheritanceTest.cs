using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.IntegrationTest.TestingUtils;
using TypeGen.IntegrationTest.TsInterfaceInheritance.Entities;
using Xunit;

namespace TypeGen.IntegrationTest.TsInterfaceInheritance;

public class TsInterfaceInheritanceTest : GenerationTestBase
{
    [Fact]
    public async Task cs_classes_which_are_ts_interfaces_should_respect_ts_interface_inheritance()
    {
        var type = typeof(Sub);
        const string expectedLocation = "TypeGen.IntegrationTest.TsInterfaceInheritance.Expected.sub.ts";
        var generationSpec = new TsInterfaceInheritanceGenerationSpec();
        var generatorOptions = new GeneratorOptions();
        
        await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
    }

    private class TsInterfaceInheritanceGenerationSpec : GenerationSpec
    {
        public TsInterfaceInheritanceGenerationSpec()
        {
            AddInterface<Base>();
            AddInterface<Sub>();
        }
    }
}