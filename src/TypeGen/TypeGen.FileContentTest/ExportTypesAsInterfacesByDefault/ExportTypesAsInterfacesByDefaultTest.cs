using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.ExportTypesAsInterfacesByDefault.Entities;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;

namespace TypeGen.FileContentTest.ExportTypesAsInterfacesByDefault;

public class ExportTypesAsInterfacesByDefaultTest : GenerationTestBase
{
    [Fact]
    public async Task if_option_not_set_should_generate_class()
    {
        var type = typeof(Child);
        const string expectedLocation = "TypeGen.FileContentTest.ExportTypesAsInterfacesByDefault.Expected.child-as-class.ts";
        var generatorOptions = new GeneratorOptions();
        var generationSpec = new ExportTypesAsInterfacesByDefaultGenerationSpec();
        await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
    }
    
    [Fact]
    public async Task if_option_set_should_generate_interface()
    {
        var type = typeof(Child);
        const string expectedLocation = "TypeGen.FileContentTest.ExportTypesAsInterfacesByDefault.Expected.child-as-interface.ts";
        var generatorOptions = new GeneratorOptions { ExportTypesAsInterfacesByDefault = true };
        var generationSpec = new ExportTypesAsInterfacesByDefaultGenerationSpec();
        await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
    }

    private class ExportTypesAsInterfacesByDefaultGenerationSpec : GenerationSpec
    {
        public ExportTypesAsInterfacesByDefaultGenerationSpec()
        {
            AddClass<Parent>();
        }
    }
}