using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NSubstitute;
using TypeGen.Cli.Business;
using TypeGen.Core.Extensions;
using TypeGen.Core.Storage;
using Xunit;

namespace TypeGen.Core.Test.Acceptance
{
    public class GeneratorAcceptanceTest
    {
        private const string ProjectPath = "../../../../TypeGen.TestWebApp/";
        private const string AssemblyPath = ProjectPath + "bin/Release/netcoreapp2.0/TypeGen.TestWebApp.dll";

        private readonly IFileSystem _fileSystem = Substitute.For<IFileSystem>();

//        [Theory(Skip = "This test should be run only in local environment. It's marked as skipped, because remote services (CI etc.) should not pick it up.")]
        [Theory]
		[InlineData("")]
		[InlineData("generated-typescript/")]
		[InlineData("nested/directory/generated-typescript/")]
        public void Generate_AssemblyGiven_TypeScriptContentGenerated(string outputPath)
        {
            //arrange
            
            var generator = new Generator(_fileSystem) { Options = { BaseOutputDirectory = outputPath, CreateIndexFile = true } };
            Assembly assembly = Assembly.LoadFrom(AssemblyPath);
            var assemblyResolver = new AssemblyResolver(new FileSystem(), ProjectPath);
            
            var content = new Dictionary<string, string>
            {
                { "bar.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.bar.ts") },
                { "base-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.base-class.ts") },
                { "c.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.c.ts") },
                { "custom-base-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.custom-base-class.ts") },
                { "custom-base-custom-import.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.custom-base-custom-import.ts") },
                { "custom-empty-base-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.custom-empty-base-class.ts") },
                { "d.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.d.ts") },
                { "e-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.e-class.ts") },
                { "external-deps-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.external-deps-class.ts") },
                { "f-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.f-class.ts") },
                { "foo-type.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.foo-type.ts") },
                { "foo.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.foo.ts") },
                { "generic-base-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.generic-base-class.ts") },
                { "generic-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.generic-class.ts") },
                { "index.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.index.ts") },
                { "lite-db-entity.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.lite-db-entity.ts") },
                { "standalone-enum.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.standalone-enum.ts") },
                { "strict-nulls-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.strict-nulls-class.ts") },
                { "with-generic-base-class-custom-type.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.with-generic-base-class-custom-type.ts") },
                { "with-ignored-base-and-custom-base.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.with-ignored-base-and-custom-base.ts") },
                { "with-ignored-base.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.with-ignored-base.ts") },
                
                { @"error-case2\base-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.error_case2.base-class.ts") },
                { @"error-case2\base-class2.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.error_case2.base-class2.ts") },
                { @"error-case2\my-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.error_case2.my-class.ts") },
                { @"error-case2\my-join-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.error_case2.my-join-class.ts") },
                
                { @"no\slash\output\dir\no-slash-output-dir.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.no.slash.output.dir.no-slash-output-dir.ts") },
                
                { @"test-classes\base-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.test_classes.base-class.ts") },
                { @"test-classes\base-class2.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.test_classes.base-class2.ts") },
                { @"test-classes\circular-ref-class1.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.test_classes.circular-ref-class1.ts") },
                { @"test-classes\circular-ref-class2.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.test_classes.circular-ref-class2.ts") },
                { @"test-classes\test-class.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.test_classes.test-class.ts") },
                
                { @"test-enums\test-enum.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.test_enums.test-enum.ts") },
                
                { @"test-interfaces\test-interface.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.test_interfaces.test-interface.ts") },
                
                { @"very\nested\directory\nested-entity.ts", GetEmbeddedResource("TypeGen.Core.Test.Acceptance.Expected.very.nested.directory.nested-entity.ts") },
            };
            
            //act
            
            assemblyResolver.Register();
            generator.Generate(assembly);
            assemblyResolver.Unregister();
            
            //assert
            
            _fileSystem.Received().SaveFile(outputPath + "bar.ts", content["bar.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "base-class.ts", content["base-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "c.ts", content["c.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "custom-base-class.ts", content["custom-base-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "custom-base-custom-import.ts", content["custom-base-custom-import.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "custom-empty-base-class.ts", content["custom-empty-base-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "d.ts", content["d.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "e-class.ts", content["e-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "external-deps-class.ts", content["external-deps-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "f-class.ts", content["f-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "foo-type.ts", content["foo-type.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "foo.ts", content["foo.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "generic-base-class.ts", content["generic-base-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "generic-class.ts", content["generic-class.ts"]);
//            _fileSystem.Received().SaveFile(outputPath + "index.ts", content["index.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "lite-db-entity.ts", content["lite-db-entity.ts"]);
            _fileSystem.Received().SaveFile(outputPath + "standalone-enum.ts", content["standalone-enum.ts"]);
//            _fileSystem.Received().SaveFile(outputPath + "strict-nulls-class.ts", content["strict-nulls-class.ts"]);
//            _fileSystem.Received().SaveFile(outputPath + "with-generic-base-class-custom-type.ts", content["with-generic-base-class-custom-type.ts"]);
//            _fileSystem.Received().SaveFile(outputPath + "with-ignored-base-and-custom-base.ts", content["with-ignored-base-and-custom-base.ts"]);
//            _fileSystem.Received().SaveFile(outputPath + "with-ignored-base.ts", content["with-ignored-base.ts"]);
            
            _fileSystem.Received().SaveFile(outputPath + @"error-case2\base-class.ts", content[@"error-case2\base-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + @"error-case2\base-class2.ts", content[@"error-case2\base-class2.ts"]);
            _fileSystem.Received().SaveFile(outputPath + @"error-case2\my-class.ts", content[@"error-case2\my-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + @"error-case2\my-join-class.ts", content[@"error-case2\my-join-class.ts"]);
            
            _fileSystem.Received().SaveFile(outputPath + @"no\slash\output\dir\no-slash-output-dir.ts", content[@"no\slash\output\dir\no-slash-output-dir.ts"]);
            
            _fileSystem.Received().SaveFile(outputPath + @"test-classes\base-class.ts", content[@"test-classes\base-class.ts"]);
            _fileSystem.Received().SaveFile(outputPath + @"test-classes\base-class2.ts", content[@"test-classes\base-class2.ts"]);
            _fileSystem.Received().SaveFile(outputPath + @"test-classes\circular-ref-class1.ts", content[@"test-classes\circular-ref-class1.ts"]);
            _fileSystem.Received().SaveFile(outputPath + @"test-classes\circular-ref-class2.ts", content[@"test-classes\circular-ref-class2.ts"]);
            _fileSystem.Received().SaveFile(outputPath + @"test-classes\test-class.ts", content[@"test-classes\test-class.ts"]);
            
            _fileSystem.Received().SaveFile(outputPath + @"test-enums\test-enum.ts", content[@"test-enums\test-enum.ts"]);
            
            _fileSystem.Received().SaveFile(outputPath + @"test-interfaces\test-interface.ts", content[@"test-interfaces\test-interface.ts"]);
            
            _fileSystem.Received().SaveFile(outputPath + @"very\nested\directory\nested-entity.ts", content[@"very\nested\directory\nested-entity.ts"]);
        }
        
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