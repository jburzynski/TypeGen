using System;
using System.Threading.Tasks;
using TypeGen.Core;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.IntegrationTest.NullableTranslation.Entities;
using TypeGen.IntegrationTest.TestingUtils;
using Xunit;

namespace TypeGen.IntegrationTest.NullableTranslation;

public class NullableTranslationTest : GenerationTestBase
{
    [Theory]
    [InlineData(typeof(NullableClass), "TypeGen.IntegrationTest.NullableTranslation.Expected.nullable-class.ts")]
    public async Task TestNullableTranslationGenerationSpec(Type type, string expectedLocation)
    {
        var generationSpec = new NullableTranslationGenerationSpec();
        var generatorOptions = new GeneratorOptions { CsNullableTranslation = StrictNullTypeUnionFlags.Optional };
        await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
    }
    
    private class NullableTranslationGenerationSpec : GenerationSpec
    {
        public NullableTranslationGenerationSpec()
        {
            AddClass<NullableClass>();
        }
    }
}