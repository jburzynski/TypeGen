using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.AdditionalClassProperties.Entities;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;

namespace TypeGen.FileContentTest.AdditionalClassProperties
{
    public class AdditionalClassPropertiesTest : GenerationTestBase
    {
        [Fact]
        public async Task ClassWithAdditionalClassProperties_Test()
        {
            var type = typeof(ClassWithAdditionalClassProperties);
            const string expectedLocation = "TypeGen.FileContentTest.AdditionalProperties.Expected.class-with-additional-class-properties.ts";
            var generationSpec = new ClassWithAdditionalPropertiesGenerationSpec();
            var generatorOptions = new GeneratorOptions { };
            
            await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
        }
        
        private class ClassWithAdditionalPropertiesGenerationSpec : GenerationSpec
        {
            public ClassWithAdditionalPropertiesGenerationSpec()
            {
                AddClass<ClassWithAdditionalClassProperties>()
                    .WithAdditionalProperty("public email", "string")
                    .WithAdditionalProperty("greetingMessage", "string", "'Hello, World!'")
                    .WithAdditionalProperty("emptyString", "string", "''")
                    .WithAdditionalProperty("creationDate", "Date")
                    .WithAdditionalProperty("username", "string")
                    .WithAdditionalProperty("age", "number")
                    .WithAdditionalProperty("status", "'active' | 'inactive' | 'pending'");
            }
        }
    }
}
