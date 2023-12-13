using System;
using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.Logging;
using TypeGen.Core.SpecGeneration;
using TypeGen.IntegrationTest.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.IntegrationTest.TestingUtils;

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
        var expected = (await readExpectedTask).Trim();

        Assert.True(interceptor.GeneratedOutputs.ContainsKey(type));
        Assert.Equal(expected, interceptor.GeneratedOutputs[type].Content.FormatOutput());
    }
    
    protected async Task TestGenerationSpec(Type type, string expectedLocation,
        GenerationSpec generationSpec, GeneratorOptions generatorOptions)
    {
        var readExpectedTask = EmbededResourceReader.GetEmbeddedResourceAsync(expectedLocation);
        var generator = new Core.Generator.Generator(generatorOptions, logger);
        var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);

        await generator.GenerateAsync(new[] { generationSpec });
        var expected = (await readExpectedTask).Trim();

        Assert.True(interceptor.GeneratedOutputs.ContainsKey(type));
        Assert.Equal(expected, interceptor.GeneratedOutputs[type].Content.FormatOutput());
    }
}