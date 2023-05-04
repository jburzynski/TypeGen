using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeGen.Core.SpecGeneration;
using TypeGen.IntegrationTest.Extensions;
using TypeGen.IntegrationTest.Generator;
using TypeGen.IntegrationTest.Generator.TestingUtils;
using TypeGen.IntegrationTest.Issues.CircularGenericConstraint.TestClasses;
using TypeGen.TestWebApp.CustomMappingsClassGenerationIssue;
using TypeGen.TestWebApp.GenerationSpecs;
using Xunit;

namespace TypeGen.IntegrationTest.Issues.CustomMappingsClassGeneration
{
    public class CustomMappingsClassGeneration
    {
        [Fact]
        public async Task GeneratesCorrectly()
        {
            var type = typeof(ClassWithUri);
            var expectedLocation = @"TypeGen.IntegrationTest.Issues.CustomMappingsClassGeneration.Expected.class-with-uri.ts";
            var readExpectedTask = EmbededResourceReader.GetEmbeddedResourceAsync(expectedLocation);
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
            var expected = (await readExpectedTask).Trim();

            Assert.True(interceptor.GeneratedOutputs.ContainsKey(type));
            Assert.Equal(expected, interceptor.GeneratedOutputs[type].Content.FormatOutput());
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
