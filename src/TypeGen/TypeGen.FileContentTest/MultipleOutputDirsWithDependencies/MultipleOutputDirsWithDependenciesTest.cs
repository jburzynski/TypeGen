using System;
using System.Threading.Tasks;
using FluentAssertions;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.MultipleOutputDirsWithDependencies.Entities;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;

namespace TypeGen.FileContentTest.MultipleOutputDirsWithDependencies;
    
public class MultipleOutputDirsWithDependenciesTest
{
    [Fact]
    public async Task TestMultipleOutputDirsWithDependencies()
    {
        const string class1OutputBase = "./class1/output/";
        const string class2OutputBase = "./class2/output/";
        const string class1Output = $"{class1OutputBase}class1.ts";
        const string class2Output = $"{class2OutputBase}class2.ts";
        const string interface1Output = $"{class1OutputBase}interface1.ts";
        const string interface2Output = $"{class2OutputBase}interface2.ts";
        
        var generationSpec = new MultipleOutputDirsWithDependenciesGenerationSpec();
        var generatorOptions = new GeneratorOptions();
        
        var generator = Generator.Get(generatorOptions);
        var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);

        await generator.GenerateAsync(generationSpec);

        interceptor.GeneratedOutputs.Should().ContainKey(typeof(Class1));
        interceptor.GeneratedOutputs.Should().ContainKey(typeof(Class2));
        interceptor.GeneratedOutputs.Should().ContainKey(typeof(Interface1));
        interceptor.GeneratedOutputs.Should().ContainKey(typeof(Interface2));

        interceptor.GeneratedOutputs[typeof(Class1)].Path.Should().Be(class1Output);
        interceptor.GeneratedOutputs[typeof(Class2)].Path.Should().Be(class2Output);
        interceptor.GeneratedOutputs[typeof(Interface1)].Path.Should().Be(interface1Output);
        interceptor.GeneratedOutputs[typeof(Interface2)].Path.Should().Be(interface2Output);
    }

    private class MultipleOutputDirsWithDependenciesGenerationSpec : GenerationSpec
    {
        public MultipleOutputDirsWithDependenciesGenerationSpec()
        {
            AddClass<Class1>("./class1/output");
            AddClass<Class2>("./class2/output");
        }
    }
}