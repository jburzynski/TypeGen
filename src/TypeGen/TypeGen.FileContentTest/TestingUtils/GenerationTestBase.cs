using System;
using System.Threading.Tasks;
using FluentAssertions;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.Extensions;

namespace TypeGen.FileContentTest.TestingUtils;

public class GenerationTestBase
{
    protected async Task TestFromAssembly(Type type, string expectedLocation)
    {
        var readExpectedTask = EmbededResourceReader.GetEmbeddedResourceAsync(expectedLocation);

        var generator = Generator.Get();
        var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);

        await generator.GenerateAsync(type.Assembly);
        var expected = (await readExpectedTask).NormalizeFileContent();

        interceptor.GeneratedOutputs.Should().ContainKey(type);
        interceptor.GeneratedOutputs[type].Content.NormalizeFileContent().Should().Be(expected);
    }
    
    protected static async Task TestGenerationSpec(Type type, string expectedLocation,
        GenerationSpec generationSpec, GeneratorOptions generatorOptions)
    {
        var readExpectedTask = EmbededResourceReader.GetEmbeddedResourceAsync(expectedLocation);
        var generator = Generator.Get(generatorOptions);
        var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);

        await generator.GenerateAsync(new[] { generationSpec });
        var expected = (await readExpectedTask).NormalizeFileContent();

        interceptor.GeneratedOutputs.Should().ContainKey(type);
        interceptor.GeneratedOutputs[type].Content.NormalizeFileContent().Should().Be(expected);
    }
}