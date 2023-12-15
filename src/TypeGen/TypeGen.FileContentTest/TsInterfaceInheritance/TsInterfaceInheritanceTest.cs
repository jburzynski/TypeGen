using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.TestingUtils;
using TypeGen.FileContentTest.TsInterfaceInheritance.Entities;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.FileContentTest.TsInterfaceInheritance;

public class TsInterfaceInheritanceTest : GenerationTestBase
{
    public TsInterfaceInheritanceTest(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task cs_classes_which_are_ts_interfaces_should_respect_ts_interface_inheritance()
    {
        var type = typeof(Sub);
        const string expectedLocation = "TypeGen.FileContentTest.TsInterfaceInheritance.Expected.sub.ts";
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