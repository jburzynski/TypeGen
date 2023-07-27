using System;
using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.IntegrationTest.TestingUtils;
using Xunit;

namespace TypeGen.IntegrationTest.ConstantsOnly;

public class ConstantsOnlyTest : GenerationTestBase
{
    [Theory]
    [InlineData(typeof(Entities.ConstantsOnly), "TypeGen.IntegrationTest.ConstantsOnly.Expected.constants-only.ts")]
    public async Task TestConstantsOnlyGenerationSpec(Type type, string expectedLocation)
    {
        var generationSpec = new ConstantsOnlyGenerationSpec();
        var generatorOptions = new GeneratorOptions { CsDefaultValuesForConstantsOnly = true };
        await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
    }
    
    private class ConstantsOnlyGenerationSpec : GenerationSpec
    {
        public ConstantsOnlyGenerationSpec()
        {
            AddClass<Entities.ConstantsOnly>();
        }
    }
}