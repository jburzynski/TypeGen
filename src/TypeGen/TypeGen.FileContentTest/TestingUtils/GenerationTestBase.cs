using System;
using System.Threading.Tasks;
using FluentAssertions;
using TypeGen.Core.Generator;
using TypeGen.Core.Logging;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.Extensions;
using TypeGen.IntegrationTest.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.FileContentTest.TestingUtils;

public class GenerationTestBase
{
    private class TestLogger : ILogger
    {
        private readonly ITestOutputHelper output;

        public TestLogger(ITestOutputHelper output)
        {
            this.output = output;
        }

        void ILogger.Log(string message, LogLevel level)
        {
            output.WriteLine(message); 
        }
    }

    private readonly ITestOutputHelper output;
    private readonly ILogger logger;
    protected GenerationTestBase(ITestOutputHelper output)
    {
        this.output = output;
        this.logger = new TestLogger(output);
    }

    protected async Task TestFromAssembly(Type type, string expectedLocation)
    {
        var readExpectedTask = EmbededResourceReader.GetEmbeddedResourceAsync(expectedLocation);

        GeneratorOptions options = new GeneratorOptions();
        options.FileNameConverters.Add(new AddFolderConverter());

        var generator = new Generator(options, logger);
        var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);

        await generator.GenerateAsync(type.Assembly);
        var expected = (await readExpectedTask).NormalizeFileContent();

        interceptor.GeneratedOutputs.Should().ContainKey(type);
        interceptor.GeneratedOutputs[type].Content.NormalizeFileContent().Should().Be(expected);
    }
    
    protected async Task TestGenerationSpec(Type type, string expectedLocation,
        GenerationSpec generationSpec, GeneratorOptions generatorOptions)
    {
        var readExpectedTask = EmbededResourceReader.GetEmbeddedResourceAsync(expectedLocation);
        var generator = new Generator(generatorOptions, logger);
        var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);

        await generator.GenerateAsync(new[] { generationSpec });
        var expected = (await readExpectedTask).NormalizeFileContent();

        interceptor.GeneratedOutputs.Should().ContainKey(type);
        interceptor.GeneratedOutputs[type].Content.NormalizeFileContent().Should().Be(expected);
    }
}