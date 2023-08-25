using System;
using System.Threading.Tasks;
using TypeGen.IntegrationTest.CommonCases;
using TypeGen.IntegrationTest.DefaultExport.Entities;
using TypeGen.IntegrationTest.TestingUtils;
using Xunit;

namespace TypeGen.IntegrationTest.DefaultExport
{
    public class DefaultExportTest : GenerationTestBase
    {
        /// <summary>
        /// Looks into generating classes and interfaces with circular type constraints
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expectedLocation"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(typeof(ClassWithDefaultExport), "TypeGen.IntegrationTest.DefaultExport.Expected.class-with-default-export.ts")]
        [InlineData(typeof(GenericClassWithDefaultExport<,>), "TypeGen.IntegrationTest.DefaultExport.Expected.generic-class-with-default-export.ts")]
        [InlineData(typeof(ClassWithImports), "TypeGen.IntegrationTest.DefaultExport.Expected.class-with-imports.ts")]
        [InlineData(typeof(ClassWithoutDefaultExport), "TypeGen.IntegrationTest.DefaultExport.Expected.class-without-default-export.ts")]
        [InlineData(typeof(InterfaceWithDefaultExport), "TypeGen.IntegrationTest.DefaultExport.Expected.interface-with-default-export.ts")]
        
        // structs
        
        [InlineData(typeof(DefaultExport.Entities.Structs.ClassWithDefaultExport), "TypeGen.IntegrationTest.DefaultExport.Expected.class-with-default-export.ts")]
        [InlineData(typeof(DefaultExport.Entities.Structs.GenericClassWithDefaultExport<,>), "TypeGen.IntegrationTest.DefaultExport.Expected.generic-class-with-default-export.ts")]
        [InlineData(typeof(DefaultExport.Entities.Structs.ClassWithImports), "TypeGen.IntegrationTest.DefaultExport.Expected.class-with-imports.ts")]
        [InlineData(typeof(DefaultExport.Entities.Structs.ClassWithoutDefaultExport), "TypeGen.IntegrationTest.DefaultExport.Expected.class-without-default-export.ts")]
        [InlineData(typeof(DefaultExport.Entities.Structs.InterfaceWithDefaultExport), "TypeGen.IntegrationTest.DefaultExport.Expected.interface-with-default-export.ts")]
        public async Task GeneratesCorrectly(Type type, string expectedLocation)
        {
            await TestFromAssembly(type, expectedLocation);
        }
    }
}
