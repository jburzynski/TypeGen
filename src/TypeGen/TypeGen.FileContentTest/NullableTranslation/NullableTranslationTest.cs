using System;
using System.Threading.Tasks;
using TypeGen.Core;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.NullableTranslation.Entities;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.FileContentTest.NullableTranslation;

public class NullableTranslationTest : GenerationTestBase
{
    public NullableTranslationTest(ITestOutputHelper output) : base(output) { }

    [Theory]
    [InlineData(typeof(NullableClass), "TypeGen.FileContentTest.NullableTranslation.Expected.nullable-class.ts")]
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