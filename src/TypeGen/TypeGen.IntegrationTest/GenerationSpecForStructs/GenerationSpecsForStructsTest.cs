using System;
using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.IntegrationTest.CommonCases.Entities.Structs;
using TypeGen.IntegrationTest.TestingUtils;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.IntegrationTest.GenerationSpecForStructs;

public class GenerationSpecsForStructsTest : GenerationTestBase
{
    public GenerationSpecsForStructsTest(ITestOutputHelper output) : base(output) { }

    [Theory]
    [InlineData(typeof(CustomBaseClass), "TypeGen.IntegrationTest.CommonCases.Expected.custom-base-class.ts")]
    [InlineData(typeof(CustomBaseCustomImport), "TypeGen.IntegrationTest.CommonCases.Expected.custom-base-custom-import.ts")]
    [InlineData(typeof(CustomEmptyBaseClass), "TypeGen.IntegrationTest.CommonCases.Expected.custom-empty-base-class.ts")]
    [InlineData(typeof(ExtendedPrimitivesClass), "TypeGen.IntegrationTest.CommonCases.Expected.extended-primitives-class.ts")]
    [InlineData(typeof(ExternalDepsClass), "TypeGen.IntegrationTest.CommonCases.Expected.external-deps-class.ts")]
    [InlineData(typeof(GenericBaseClass<>), "TypeGen.IntegrationTest.CommonCases.Expected.generic-base-class.ts")]
    public async Task TestGenerationSpecForStructs(Type type, string expectedLocation)
    {
        var generationSpec = new StructsGenerationSpec();
        var generatorOptions = new GeneratorOptions();
        await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
    }
    
    private class StructsGenerationSpec : GenerationSpec
    {
        public StructsGenerationSpec()
        {
            AddClass<CustomBaseClass>().CustomBase("AcmeCustomBase<string>");
            AddInterface<CustomBaseCustomImport>().CustomBase("MB", "./my/base/my-base", "MyBase");
            AddInterface<CustomEmptyBaseClass>().CustomBase();
            AddClass<ExtendedPrimitivesClass>()
                .Member(x => nameof(x.DateTimeStringField))
                .Type("string")
                .Member(x => nameof(x.DateTimeOffsetStringField))
                .Type("string");
            AddClass<ExternalDepsClass>().Member(nameof(ExternalDepsClass.User)).Ignore();
            AddClass(typeof(GenericBaseClass<>));
            AddClass(typeof(GenericWithRestrictions<>));
            AddClass<LiteDbEntity>().Member(nameof(LiteDbEntity.MyBsonArray)).Ignore();
        }
    }
}