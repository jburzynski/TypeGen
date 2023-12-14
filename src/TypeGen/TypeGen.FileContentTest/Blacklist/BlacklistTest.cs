using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using TypeGen.Core;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.Blacklist.Entities;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;

namespace TypeGen.FileContentTest.Blacklist
{
    public class BlacklistTest : GenerationTestBase
    {
        [Fact]
        public async Task ClassWithBlacklistedBase_Test()
        {
            var type = typeof(ClassWithBlacklistedBase);
            const string expectedLocation = "TypeGen.FileContentTest.Blacklist.Expected.class-with-blacklisted-base.ts";
            var blacklist = new HashSet<string> { typeof(Bar).FullName };
            var generationSpec = new ClassWithBlacklistedBaseGenerationSpec();
            var generatorOptions = new GeneratorOptions { TypeBlacklist = blacklist };
            
            await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
        }
        
        [Fact]
        public async Task ClassWithBlacklistedInterface_Test()
        {
            var type = typeof(ClassWithBlacklistedInterface);
            const string expectedLocation = "TypeGen.FileContentTest.Blacklist.Expected.class-with-blacklisted-interface.ts";
            var blacklist = new HashSet<string> { typeof(IFoo).FullName };
            var generationSpec = new ClassWithBlacklistedInterfaceGenerationSpec();
            var generatorOptions = new GeneratorOptions { TypeBlacklist = blacklist };
            
            await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
        }
        
        [Fact]
        public async Task InterfaceWithBlacklistedBase_Test()
        {
            var type = typeof(InterfaceWithBlacklistedBase);
            const string expectedLocation = "TypeGen.FileContentTest.Blacklist.Expected.interface-with-blacklisted-base.ts";
            var blacklist = new HashSet<string> { typeof(IFoo).FullName };
            var generationSpec = new InterfaceWithBlacklistedBaseGenerationSpec();
            var generatorOptions = new GeneratorOptions { TypeBlacklist = blacklist };
            
            await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
        }
        
        [Fact]
        public void ClassWithBlacklistedPropertyType_Test()
        {
            var blacklist = new HashSet<string> { typeof(Baz).FullName };
            var generatorOptions = new GeneratorOptions { TypeBlacklist = blacklist };
            var generator = new Generator(generatorOptions);
            var generationSpec = new ClassWithBlacklistedPropertyTypeGenerationSpec();

            ((Action)(() => generator.Generate(new[] { generationSpec }))).Should().Throw<CoreException>();
        }
        
        [Fact]
        public void ClassWithBlacklistedTypeInDictionary_Test()
        {
            var blacklist = new HashSet<string> { typeof(Baz).FullName };
            var generatorOptions = new GeneratorOptions { TypeBlacklist = blacklist };
            var generator = new Generator(generatorOptions);
            var generationSpec = new ClassWithBlacklistedTypeInDictionaryGenerationSpec();

            ((Action)(() => generator.Generate(new[] { generationSpec }))).Should().Throw<CoreException>();
        }
        
        [Fact]
        public void ClassWithBlacklistedTypeInArray_Test()
        {
            var blacklist = new HashSet<string> { typeof(Baz).FullName };
            var generatorOptions = new GeneratorOptions { TypeBlacklist = blacklist };
            var generator = new Generator(generatorOptions);
            var generationSpec = new ClassWithBlacklistedTypeInArrayGenerationSpec();

            ((Action)(() => generator.Generate(new[] { generationSpec }))).Should().Throw<CoreException>();
        }
        
        [Fact]
        public void ClassWithBlacklistedTypeInCustomGeneric_Test()
        {
            var blacklist = new HashSet<string> { typeof(Baz).FullName };
            var generatorOptions = new GeneratorOptions { TypeBlacklist = blacklist };
            var generator = new Generator(generatorOptions);
            var generationSpec = new ClassWithBlacklistedTypeInCustomGenericGenerationSpec();

            ((Action)(() => generator.Generate(new[] { generationSpec }))).Should().Throw<CoreException>();
        }
        
        [Fact]
        public void InterfaceWithBlacklistedPropertyType_Test()
        {
            var blacklist = new HashSet<string> { typeof(Baz).FullName };
            var generatorOptions = new GeneratorOptions { TypeBlacklist = blacklist };
            var generator = new Generator(generatorOptions);
            var generationSpec = new InterfaceWithBlacklistedPropertyTypeGenerationSpec();

            ((Action)(() => generator.Generate(new[] { generationSpec }))).Should().Throw<CoreException>();
        }
        
        [Fact]
        public async Task Record_Test()
        {
            var type = typeof(MyRecord);
            const string expectedLocation = "TypeGen.FileContentTest.Blacklist.Expected.my-record.ts";
            var generationSpec = new RecordGenerationSpec();
            var generatorOptions = new GeneratorOptions();
            
            await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
        }

        private class ClassWithBlacklistedBaseGenerationSpec : GenerationSpec
        {
            public ClassWithBlacklistedBaseGenerationSpec() => AddClass<ClassWithBlacklistedBase>();
        }
        
        private class ClassWithBlacklistedInterfaceGenerationSpec : GenerationSpec
        {
            public ClassWithBlacklistedInterfaceGenerationSpec() => AddClass<ClassWithBlacklistedInterface>();
        }
        
        private class InterfaceWithBlacklistedBaseGenerationSpec : GenerationSpec
        {
            public InterfaceWithBlacklistedBaseGenerationSpec() => AddInterface<InterfaceWithBlacklistedBase>();
        }
        
        private class ClassWithBlacklistedPropertyTypeGenerationSpec : GenerationSpec
        {
            public ClassWithBlacklistedPropertyTypeGenerationSpec() => AddClass<ClassWithBlacklistedPropertyType>();
        }
        
        private class ClassWithBlacklistedTypeInDictionaryGenerationSpec : GenerationSpec
        {
            public ClassWithBlacklistedTypeInDictionaryGenerationSpec() => AddClass<ClassWithBlacklistedTypeInDictionary>();
        }
        
        private class ClassWithBlacklistedTypeInArrayGenerationSpec : GenerationSpec
        {
            public ClassWithBlacklistedTypeInArrayGenerationSpec() => AddClass<ClassWithBlacklistedTypeInArray>();
        }
        
        private class ClassWithBlacklistedTypeInCustomGenericGenerationSpec : GenerationSpec
        {
            public ClassWithBlacklistedTypeInCustomGenericGenerationSpec() => AddClass<ClassWithBlacklistedTypeInCustomGeneric>();
        }
        
        private class InterfaceWithBlacklistedPropertyTypeGenerationSpec : GenerationSpec
        {
            public InterfaceWithBlacklistedPropertyTypeGenerationSpec() => AddClass<InterfaceWithBlacklistedPropertyType>();
        }
        
        private class RecordGenerationSpec : GenerationSpec
        {
            public RecordGenerationSpec() => AddClass<MyRecord>();
        }
    }
}
