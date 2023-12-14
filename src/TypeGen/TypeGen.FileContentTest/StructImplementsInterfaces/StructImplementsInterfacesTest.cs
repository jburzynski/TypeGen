using System;
using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.StructImplementsInterfaces.Entities;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.FileContentTest.StructImplementsInterfaces;

public class StructImplementsInterfacesTest : GenerationTestBase
{
    public StructImplementsInterfacesTest(ITestOutputHelper output) : base(output) {}

    [Theory]
    [InlineData(typeof(ImplementsInterfaces), "TypeGen.FileContentTest.StructImplementsInterfaces.Expected.implements-interfaces.ts")]
    public async Task TestStructImplementsInterfacesFromGenerationSpec(Type type, string expectedLocation)
    {
        var generationSpec = new StructImplementsInterfacesGenerationSpec();
        var generatorOptions = new GeneratorOptions();
        await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
    }
    
    [Theory]
    [InlineData(typeof(ImplementsInterfaces), "TypeGen.FileContentTest.StructImplementsInterfaces.Expected.implements-interfaces.ts")]
    public async Task TestStructImplementsInterfacesFromAssembly(Type type, string expectedLocation)
    {
        await TestFromAssembly(type, expectedLocation);
    }
    
    private class StructImplementsInterfacesGenerationSpec : GenerationSpec
    {
        public StructImplementsInterfacesGenerationSpec()
        {
            AddClass<ImplementsInterfaces>();
        }
    }
}