using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NSubstitute;
using TypeGen.Cli.Business;
using TypeGen.Core;
using TypeGen.Core.Business;
using TypeGen.Core.Extensions;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.Storage;
using TypeGen.TestWebApp.TestEntities;
using Xunit;

namespace TypeGen.AcceptanceTest.Generator
{
    public class GeneratorAcceptanceTest
    {
        private const string ProjectPath = "../../../../TypeGen.TestWebApp/";

        private readonly IFileSystem _fileSystem = Substitute.For<IFileSystem>();
        
        private IDictionary<string, string> Content => new Dictionary<string, string>
        {
            { "foo-constants.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.foo-constants.ts") },
            { "bar.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.bar.ts") },
            { "base-class.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.base-class.ts") },
            { "base-class2.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.base-class2.ts") },
            { "c.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.c.ts") },
            { "custom-base-class.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.custom-base-class.ts") },
            { "custom-base-custom-import.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.custom-base-custom-import.ts") },
            { "custom-empty-base-class.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.custom-empty-base-class.ts") },
            { "extended-primitives-class.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.extended-primitives-class.ts") },
            { "d.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.d.ts") },
            { "e-class.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.e-class.ts") },
            { "external-deps-class.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.external-deps-class.ts") },
            { "f-class.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.f-class.ts") },
            { "foo-type.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.foo-type.ts") },
            { "foo.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.foo.ts") },
            { "generic-base-class.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.generic-base-class.ts") },
            { "generic-class.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.generic-class.ts") },
            { "generic-with-restrictions.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.generic-with-restrictions.ts") },
//                { "index.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.index.ts") },
            { "lite-db-entity.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.lite-db-entity.ts") },
            { "readonly-interface.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.readonly-interface.ts") },
            { "standalone-enum.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.standalone-enum.ts") },
            { "static-readonly.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.static-readonly.ts") },
            { "strict-nulls-class.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.strict-nulls-class.ts") },
            { "with-generic-base-class-custom-type.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.with-generic-base-class-custom-type.ts") },
            { "with-ignored-base-and-custom-base.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.with-ignored-base-and-custom-base.ts") },
            { "with-ignored-base.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.with-ignored-base.ts") },
            
            { @"no\slash\output\dir\no-slash-output-dir.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.no.slash.output.dir.no-slash-output-dir.ts") },
            
            { @"test-classes\base-class.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.test_classes.base-class.ts") },
            { @"test-classes\base-class2.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.test_classes.base-class2.ts") },
            { @"test-classes\circular-ref-class1.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.test_classes.circular-ref-class1.ts") },
            { @"test-classes\circular-ref-class2.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.test_classes.circular-ref-class2.ts") },
            { @"test-classes\test-class.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.test_classes.test-class.ts") },
            
            { @"test-enums\test-enum.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.test_enums.test-enum.ts") },
            
            { @"test-interfaces\test-interface.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.test_interfaces.test-interface.ts") },
            
            { @"very\nested\directory\nested-entity.ts", GetEmbeddedResource("TypeGen.AcceptanceTest.Generator.Expected.very.nested.directory.nested-entity.ts") },
        };

        #region Assembly
        
        [Theory(Skip = "This test should be run only in local environment. It's marked as skipped, because remote services (build servers etc.) should not pick it up.")]
//        [Theory]
		[InlineData("")]
		[InlineData("generated-typescript/")]
		[InlineData("nested/directory/generated-typescript/")]
        public void Generate_AssemblyGiven_TypeScriptContentGenerated(string outputPath)
        {
            //arrange
            
            const string assemblyPath = ProjectPath + "bin/Debug/netcoreapp2.0/TypeGen.TestWebApp.dll";
            
            var generator = new Core.Generator(_fileSystem) { Options = { BaseOutputDirectory = outputPath, CreateIndexFile = true, } };
            
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            var assemblyResolver = new AssemblyResolver(new FileSystem(), new ConsoleLogger(), ProjectPath);
            
            //act
            
            assemblyResolver.Register();
            generator.Generate(assembly);
            assemblyResolver.Unregister();

            //assert

//            _fileSystem.Received().SaveFile(outputPath + "foo-constants.ts", Content["foo-constants.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "bar.ts", Content["bar.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "base-class.ts", Content["base-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "base-class2.ts", Content["base-class2.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "c.ts", Content["c.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "custom-base-class.ts", Content["custom-base-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "custom-base-custom-import.ts", Content["custom-base-custom-import.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "custom-empty-base-class.ts", Content["custom-empty-base-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "extended-primitives-class.ts", Content["extended-primitives-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "d.ts", Content["d.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "e-class.ts", Content["e-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "external-deps-class.ts", Content["external-deps-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "f-class.ts", Content["f-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "foo-type.ts", Content["foo-type.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "foo.ts", Content["foo.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "generic-base-class.ts", Content["generic-base-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "generic-class.ts", Content["generic-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "generic-with-restrictions.ts", Content["generic-with-restrictions.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "index.ts", Arg.Any<string>()); // any content, because file order in index.ts is different each time
            _fileSystem.Received().SaveFile(outputPath + "lite-db-entity.ts", Content["lite-db-entity.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "readonly-interface.ts", Content["readonly-interface.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "standalone-enum.ts", Content["standalone-enum.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "static-readonly.ts", Content["static-readonly.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "strict-nulls-class.ts", Content["strict-nulls-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "with-generic-base-class-custom-type.ts", Content["with-generic-base-class-custom-type.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "with-ignored-base-and-custom-base.ts", Content["with-ignored-base-and-custom-base.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "with-ignored-base.ts", Content["with-ignored-base.ts"]);
            
            _fileSystem.Received().SaveFile(outputPath + @"no/slash/output/dir\no-slash-output-dir.ts", Content[@"no\slash\output\dir\no-slash-output-dir.ts"]);
            
            _fileSystem.Received().SaveFile(outputPath + @"test-classes\base-class.ts", Content[@"test-classes\base-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + @"test-classes\base-class2.ts", Content[@"test-classes\base-class2.ts"]);
            _fileSystem.Received().SaveFile(outputPath + @"test-classes\circular-ref-class1.ts", Content[@"test-classes\circular-ref-class1.ts"]);
            _fileSystem.Received().SaveFile(outputPath + @"test-classes\circular-ref-class2.ts", Content[@"test-classes\circular-ref-class2.ts"]);
            _fileSystem.Received().SaveFile(outputPath + @"test-classes\test-class.ts", Content[@"test-classes\test-class.ts"]);
            
            _fileSystem.Received().SaveFile(outputPath + @"test-enums\test-enum.ts", Content[@"test-enums\test-enum.ts"]);
            
            _fileSystem.Received().SaveFile(outputPath + @"test-interfaces\test-interface.ts", Content[@"test-interfaces\test-interface.ts"]);
            
            _fileSystem.Received().SaveFile(outputPath + @"./very/nested/directory/nested-entity.ts", Content[@"very\nested\directory\nested-entity.ts"]);
        }
        
        #endregion
        
        #region GenerationSpec
        
        [Theory(Skip = "This test should be run only in local environment. It's marked as skipped, because remote services (build servers etc.) should not pick it up.")]
//        [Theory]
        [InlineData("")]
        [InlineData("generated-typescript/")]
        [InlineData("nested/directory/generated-typescript/")]
        public void Generate_GenerationSpecGiven_TypeScriptContentGenerated(string outputPath)
        {
            //arrange
            
            var generator = new Core.Generator(_fileSystem) { Options = { BaseOutputDirectory = outputPath, CreateIndexFile = true } };
            var assemblyResolver = new AssemblyResolver(new FileSystem(), new ConsoleLogger(), ProjectPath);
            var generationSpec = new AcceptanceTestGenerationSpec();
            
            //act
            
            assemblyResolver.Register();
            generator.Generate(generationSpec);
            assemblyResolver.Unregister();
            
            //assert
            
            _fileSystem.Received().SaveFile(outputPath + "custom-base-class.ts", Content["custom-base-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "custom-base-custom-import.ts", Content["custom-base-custom-import.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "custom-empty-base-class.ts", Content["custom-empty-base-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "extended-primitives-class.ts", Content["extended-primitives-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "external-deps-class.ts", Content["external-deps-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "generic-base-class.ts", Content["generic-base-class.ts"]);
        }
        
        private class AcceptanceTestGenerationSpec : GenerationSpec
        {
            public AcceptanceTestGenerationSpec()
            {
                AddClass<CustomBaseClass>().CustomBase("AcmeCustomBase<string>");
                AddInterface<CustomBaseCustomImport>().CustomBase("MB", "./my/base/my-base", "MyBase");
                AddInterface<CustomEmptyBaseClass>().CustomBase();
                AddClass<ExtendedPrimitivesClass>();
                AddClass<ExternalDepsClass>().Member(nameof(ExternalDepsClass.User)).Ignore();
                AddClass(typeof(GenericBaseClass<>));
                AddClass(typeof(GenericClass<>));
                AddClass(typeof(GenericWithRestrictions<>));
                AddClass<LiteDbEntity>().Member(nameof(LiteDbEntity.MyBsonArray)).Ignore();
                AddInterface<NestedEntity>("./very/nested/directory/").Member(nameof(NestedEntity.OptionalProperty)).Optional();
            }
        }
        
        #endregion
        
        private string GetEmbeddedResource(string name)
        {
            using (Stream stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                {
                    throw new CoreException($"Could not find embedded resource '{name}'");
                }

                var contentBytes = new byte[stream.Length];
                stream.Read(contentBytes, 0, (int)stream.Length);
                return Encoding.ASCII.GetString(contentBytes);
            }
        }
    }
}