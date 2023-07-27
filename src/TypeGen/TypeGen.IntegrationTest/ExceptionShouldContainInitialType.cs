using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeGen.Core;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using Xunit;

namespace TypeGen.IntegrationTest;

public class ExceptionShouldContainInitialType
{
    /// <summary>
    /// Tests exception containing initial type name is thrown when dependency type fails
    /// </summary>
    [Fact]
    public async Task ShouldThrowExceptionWithInitialTypeNameWhenDependencyTypeFails()
    {
        var type = typeof(TestExceptions);
        var spec = new ExceptionsGenerationSpec();
        var generator = new Generator();
        try
        {
            await generator.GenerateAsync(new[] { spec });
            Assert.True(true, "Exception not thrown");
        }
        catch (CoreException ex)
        {
            Assert.Contains(type.Name, ex.Message);
            Assert.NotNull(ex.InnerException);
            Assert.Contains("Nullable`1", ex.InnerException.Message);
        }
    }
        
    private class ExceptionsGenerationSpec : GenerationSpec
    {
        public ExceptionsGenerationSpec()
        {
            AddClass<TestExceptions>();
        }
    }
    
    private class TestExceptions
    {
        public List<Guid?>? InvalidCollection { get; set; }
    }
}