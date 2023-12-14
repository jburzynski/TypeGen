using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.CommonCases.Entities.CustomMappingsClassGenerationIssue;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.FileContentTest.CustomMappingsClassGeneration
{
    public class CustomMappingsClassGeneration : GenerationTestBase
    {
        public CustomMappingsClassGeneration(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task GeneratesCorrectly()
        {
            var type = typeof(ClassWithUri);
            var expectedLocation = @"TypeGen.FileContentTest.CustomMappingsClassGeneration.Expected.class-with-uri.ts";
            var spec = new TestGenerationSpec();
            var generatorOptions = new GeneratorOptions
            {
                CustomTypeMappings = new Dictionary<string, string>
                {
                    { "System.Uri", "URL" }
                }
            };
            
            await TestGenerationSpec(type, expectedLocation, spec, generatorOptions);
        }
        
        [Fact]
        public async Task ShouldNotGenerateDependencyIfItsInCustomTypeMappings()
        {
            var spec = new TestGenerationSpec();
            var generator = new Core.Generator.Generator
            {
                Options =
                {
                    CustomTypeMappings = new Dictionary<string, string>
                    {
                        { "System.Uri", "URL" }
                    }
                }
            };
            
            var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);
            await generator.GenerateAsync(new[] { spec });
            Assert.False(interceptor.GeneratedOutputs.ContainsKey(typeof(Uri)));
        }

        private class TestGenerationSpec : GenerationSpec
        {
            public TestGenerationSpec()
            {
                AddClass<ClassWithUri>();
            }
        }
    }
}
