using System;
using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.FileContentTest.ConstantsOnly;

public class ConstantsOnlyTest : GenerationTestBase
{
    public ConstantsOnlyTest(ITestOutputHelper output) : base(output) { }

    [Theory]
    [InlineData(typeof(Entities.ConstantsOnly), "TypeGen.FileContentTest.ConstantsOnly.Expected.constants-only.ts")]
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