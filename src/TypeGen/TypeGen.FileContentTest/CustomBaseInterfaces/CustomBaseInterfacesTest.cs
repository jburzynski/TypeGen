using System;
using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.TypeAnnotations;
using TypeGen.FileContentTest.CustomBaseInterfaces.Entities;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.FileContentTest.CustomBaseInterfaces;

public class CustomBaseInterfacesTest : GenerationTestBase
{
    public CustomBaseInterfacesTest(ITestOutputHelper output) : base(output) {}

    [Theory]
    [InlineData(typeof(Foo), "TypeGen.FileContentTest.CustomBaseInterfaces.Expected.foo.ts")]
    public async Task TestCustomBaseInterfacesGenerationSpec(Type type, string expectedLocation)
    {
        var generationSpec = new CustomBaseInterfacesGenerationSpec();
        var generatorOptions = new GeneratorOptions();
        await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
    }

    [Theory]
    [InlineData(typeof(Foo), "TypeGen.FileContentTest.CustomBaseInterfaces.Expected.foo.ts")]
    public async Task TestCustomBaseInterfacesFromAssembly(Type type, string expectedLocation)
    {
        await TestFromAssembly(type, expectedLocation);
    }
    
    private class CustomBaseInterfacesGenerationSpec : GenerationSpec
    {
        public CustomBaseInterfacesGenerationSpec()
        {
            AddClass<Foo>()
                .CustomBase(implementedInterfaces: new[]
                {
                    new ImplementedInterface("IFoo"),
                    new ImplementedInterface("IBar", "./my/path", "IOrig"),
                    new ImplementedInterface("IBaz", "./my/path/baz", IsDefaultExport: true),
                });
        }
    }
}