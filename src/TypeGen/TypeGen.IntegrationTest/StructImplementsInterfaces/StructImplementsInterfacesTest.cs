using System;
using System.Threading.Tasks;
using TypeGen.Core;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.TypeAnnotations;
using TypeGen.IntegrationTest.CommonCases.Entities.Structs;
using TypeGen.IntegrationTest.NullableTranslation.Entities;
using TypeGen.IntegrationTest.StructImplementsInterfaces.Entities;
using TypeGen.IntegrationTest.TestingUtils;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.IntegrationTest.StructImplementsInterfaces;

public class StructImplementsInterfacesTest : GenerationTestBase
{
    public StructImplementsInterfacesTest(ITestOutputHelper output) : base(output) {}

    [Theory]
    [InlineData(typeof(ImplementsInterfaces), "TypeGen.IntegrationTest.StructImplementsInterfaces.Expected.implements-interfaces.ts")]
    public async Task TestStructImplementsInterfacesFromGenerationSpec(Type type, string expectedLocation)
    {
        var generationSpec = new StructImplementsInterfacesGenerationSpec();
        var generatorOptions = new GeneratorOptions();
        await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
    }
    
    [Theory]
    [InlineData(typeof(ImplementsInterfaces), "TypeGen.IntegrationTest.StructImplementsInterfaces.Expected.implements-interfaces.ts")]
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