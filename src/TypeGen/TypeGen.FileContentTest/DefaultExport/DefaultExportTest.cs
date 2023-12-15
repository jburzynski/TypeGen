using System;
using System.Threading.Tasks;
using TypeGen.FileContentTest.DefaultExport.Entities;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.FileContentTest.DefaultExport
{
    public class DefaultExportTest : GenerationTestBase
    {
        public DefaultExportTest(ITestOutputHelper output) : base(output) { }

        /// <summary>
        /// Looks into generating classes and interfaces with circular type constraints
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expectedLocation"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(typeof(ClassWithDefaultExport), "TypeGen.FileContentTest.DefaultExport.Expected.class-with-default-export.ts")]
        [InlineData(typeof(GenericClassWithDefaultExport<,>), "TypeGen.FileContentTest.DefaultExport.Expected.generic-class-with-default-export.ts")]
        [InlineData(typeof(ClassWithImports), "TypeGen.FileContentTest.DefaultExport.Expected.class-with-imports.ts")]
        [InlineData(typeof(ClassWithoutDefaultExport), "TypeGen.FileContentTest.DefaultExport.Expected.class-without-default-export.ts")]
        [InlineData(typeof(InterfaceWithDefaultExport), "TypeGen.FileContentTest.DefaultExport.Expected.interface-with-default-export.ts")]
        
        // structs
        
        [InlineData(typeof(DefaultExport.Entities.Structs.ClassWithDefaultExport), "TypeGen.FileContentTest.DefaultExport.Expected.class-with-default-export.ts")]
        [InlineData(typeof(DefaultExport.Entities.Structs.GenericClassWithDefaultExport<,>), "TypeGen.FileContentTest.DefaultExport.Expected.generic-class-with-default-export.ts")]
        [InlineData(typeof(DefaultExport.Entities.Structs.ClassWithImports), "TypeGen.FileContentTest.DefaultExport.Expected.class-with-imports.ts")]
        [InlineData(typeof(DefaultExport.Entities.Structs.ClassWithoutDefaultExport), "TypeGen.FileContentTest.DefaultExport.Expected.class-without-default-export.ts")]
        [InlineData(typeof(DefaultExport.Entities.Structs.InterfaceWithDefaultExport), "TypeGen.FileContentTest.DefaultExport.Expected.interface-with-default-export.ts")]
        public async Task GeneratesCorrectly(Type type, string expectedLocation)
        {
            await TestFromAssembly(type, expectedLocation);
        }
    }
}
