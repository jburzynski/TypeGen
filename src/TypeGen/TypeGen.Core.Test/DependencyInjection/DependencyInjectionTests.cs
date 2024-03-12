using TypeGen.Core.Generator.Services;
using TypeGen.Core.Test.Fixtures;
using TypeGen.Core.TypeAnnotations;
using Xunit;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace TypeGen.Core.Test.DependencyInjection;

public class DependencyInjectionTests : TestBed<DiFixture>
{
    private Core.Generator.Generator _generator;
    
    public DependencyInjectionTests(ITestOutputHelper testOutputHelper, DiFixture fixture) : base(testOutputHelper, fixture)
    {
        _generator = fixture.GetService<Core.Generator.Generator>(testOutputHelper);
    }
    
    [Fact]
    public void DependencyInjection_Test()
    {
        var generationSpecProvider = new GenerationSpecProvider();
        var generationSpec = generationSpecProvider.GetGenerationSpec(typeof(Foo));
        
        _generator.Generate(generationSpec);
    }
    
    [ExportTsClass]
    private class Foo
    {
        public string HelloWorld { get; set; }
    }
}