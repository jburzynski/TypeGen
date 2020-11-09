using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TypeGen.AcceptanceTest.GeneratorTestingUtils;
using TypeGen.Core.Test.GeneratorTestingUtils;
using Xunit;
using Gen = TypeGen.Core.Generator;
using TestEntities = TypeGen.TestWebApp.TestEntities;
using Constants = TypeGen.TestWebApp.Constants;
using ErrorCase = TypeGen.TestWebApp.ErrorCase;
using TypeGen.Core.SpecGeneration;
using DefaultExport = TypeGen.TestWebApp.DefaultExport;

namespace TypeGen.AcceptanceTest.SelfContainedGeneratorTest
{
    public class SelfContainedGeneratorTest
    {

        /// <summary>
        /// Tests if a variaty of diffrent classes are correclty translated to typescript
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expectedLocation"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(typeof(Constants.FooConstants), "TypeGen.AcceptanceTest.Generator.Expected.foo-constants.ts")]
        [InlineData(typeof(TestEntities.CustomBaseCustomImport), "TypeGen.AcceptanceTest.Generator.Expected.custom-base-custom-import.ts")]
        [InlineData(typeof(ErrorCase.Bar), "TypeGen.AcceptanceTest.Generator.Expected.bar.ts")]
        [InlineData(typeof(TestEntities.BaseClass<>), "TypeGen.AcceptanceTest.Generator.Expected.base-class.ts")]
        [InlineData(typeof(TestEntities.BaseClass2<>), "TypeGen.AcceptanceTest.Generator.Expected.base-class2.ts")]
        [InlineData(typeof(ErrorCase.C), "TypeGen.AcceptanceTest.Generator.Expected.c.ts")]
        [InlineData(typeof(TestEntities.CustomBaseClass), "TypeGen.AcceptanceTest.Generator.Expected.custom-base-class.ts")]
        [InlineData(typeof(TestEntities.CustomEmptyBaseClass), "TypeGen.AcceptanceTest.Generator.Expected.custom-empty-base-class.ts")]
        [InlineData(typeof(ErrorCase.D), "TypeGen.AcceptanceTest.Generator.Expected.d.ts")]
        [InlineData(typeof(TestEntities.DefaultMemberValues), "TypeGen.AcceptanceTest.Generator.Expected.default-member-values.ts")]
        [InlineData(typeof(ErrorCase.EClass), "TypeGen.AcceptanceTest.Generator.Expected.e-class.ts")]
        [InlineData(typeof(TestEntities.ExtendedPrimitivesClass), "TypeGen.AcceptanceTest.Generator.Expected.extended-primitives-class.ts")]
        [InlineData(typeof(TestEntities.ExternalDepsClass), "TypeGen.AcceptanceTest.Generator.Expected.external-deps-class.ts")]
        [InlineData(typeof(ErrorCase.FClass), "TypeGen.AcceptanceTest.Generator.Expected.f-class.ts")]
        [InlineData(typeof(ErrorCase.FooType), "TypeGen.AcceptanceTest.Generator.Expected.foo-type.ts")]
        [InlineData(typeof(ErrorCase.Foo), "TypeGen.AcceptanceTest.Generator.Expected.foo.ts")]
        [InlineData(typeof(TestEntities.GenericBaseClass<>), "TypeGen.AcceptanceTest.Generator.Expected.generic-base-class.ts")]
        [InlineData(typeof(TestEntities.GenericClass<>), "TypeGen.AcceptanceTest.Generator.Expected.generic-class.ts")]
        [InlineData(typeof(TestEntities.GenericWithRestrictions<>), "TypeGen.AcceptanceTest.Generator.Expected.generic-with-restrictions.ts")]
        [InlineData(typeof(TestEntities.LiteDbEntity), "TypeGen.AcceptanceTest.Generator.Expected.lite-db-entity.ts")]
        [InlineData(typeof(TestEntities.ReadonlyInterface), "TypeGen.AcceptanceTest.Generator.Expected.readonly-interface.ts")]
        [InlineData(typeof(TestEntities.StandaloneEnum), "TypeGen.AcceptanceTest.Generator.Expected.standalone-enum.ts")]
        [InlineData(typeof(TestEntities.EnumShortValues), "TypeGen.AcceptanceTest.Generator.Expected.enum-short-values.ts")]
        [InlineData(typeof(TestEntities.StaticReadonly), "TypeGen.AcceptanceTest.Generator.Expected.static-readonly.ts")]
        [InlineData(typeof(TestEntities.StrictNullsClass), "TypeGen.AcceptanceTest.Generator.Expected.strict-nulls-class.ts")]
        [InlineData(typeof(TestEntities.TypeUnions), "TypeGen.AcceptanceTest.Generator.Expected.type-unions.ts")]
        [InlineData(typeof(TestEntities.WithGenericBaseClassCustomType), "TypeGen.AcceptanceTest.Generator.Expected.with-generic-base-class-custom-type.ts")]
        [InlineData(typeof(TestEntities.WithIgnoredBaseAndCustomBase), "TypeGen.AcceptanceTest.Generator.Expected.with-ignored-base-and-custom-base.ts")]
        [InlineData(typeof(TestEntities.WithIgnoredBase), "TypeGen.AcceptanceTest.Generator.Expected.with-ignored-base.ts")]
        [InlineData(typeof(TestEntities.NoSlashOutputDir), "TypeGen.AcceptanceTest.Generator.Expected.no.slash.output.dir.no-slash-output-dir.ts")]
        [InlineData(typeof(TestEntities.BaseClass<>), "TypeGen.AcceptanceTest.Generator.Expected.test_classes.base-class.ts")]
        [InlineData(typeof(TestEntities.BaseClass2<>), "TypeGen.AcceptanceTest.Generator.Expected.test_classes.base-class2.ts")]
        [InlineData(typeof(TestEntities.CircularRefClass1), "TypeGen.AcceptanceTest.Generator.Expected.test_classes.circular-ref-class1.ts")]
        [InlineData(typeof(TestEntities.CircularRefClass2), "TypeGen.AcceptanceTest.Generator.Expected.test_classes.circular-ref-class2.ts")]
        [InlineData(typeof(TestEntities.TestClass<,>), "TypeGen.AcceptanceTest.Generator.Expected.test_classes.test-class.ts")]
        [InlineData(typeof(TestEntities.TestEnum), "TypeGen.AcceptanceTest.Generator.Expected.test_enums.test-enum.ts")]
        [InlineData(typeof(TestEntities.TestInterface), "TypeGen.AcceptanceTest.Generator.Expected.test_interfaces.test-interface.ts")]
        [InlineData(typeof(TestEntities.NestedEntity), "TypeGen.AcceptanceTest.Generator.Expected.very.nested.directory.nested-entity.ts")]
        [InlineData(typeof(DefaultExport.ClassWithDefaultExport), "TypeGen.AcceptanceTest.Generator.Expected.default_export.class-with-default-export.ts")]
        [InlineData(typeof(DefaultExport.GenericClassWithDefaultExport<,>), "TypeGen.AcceptanceTest.Generator.Expected.default_export.generic-class-with-default-export.ts")]
        [InlineData(typeof(DefaultExport.ClassWithImports), "TypeGen.AcceptanceTest.Generator.Expected.default_export.class-with-imports.ts")]
        [InlineData(typeof(DefaultExport.ClassWithoutDefaultExport), "TypeGen.AcceptanceTest.Generator.Expected.default_export.class-without-default-export.ts")]
        [InlineData(typeof(DefaultExport.InterfaceWithDefaultExport), "TypeGen.AcceptanceTest.Generator.Expected.default_export.interface-with-default-export.ts")]
        public async Task TestGenerate(Type type, string expectedLocation)
        {
            var readExpectedTask = EmbededResourceReader.GetEmbeddedResourceAsync(expectedLocation);

            var generator = new Gen.Generator();
            var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);

            await generator.GenerateAsync(type.Assembly);
            var expected = (await readExpectedTask).Trim();

            Assert.True(interceptor.GeneratedOutputs.ContainsKey(type));
            Assert.Equal(expected, FromatOutput(interceptor.GeneratedOutputs[type].Content));
        }


        [Theory]
        [InlineData(typeof(TestEntities.CustomBaseClass), "TypeGen.AcceptanceTest.Generator.Expected.custom-base-class.ts")]
        [InlineData(typeof(TestEntities.CustomBaseCustomImport), "TypeGen.AcceptanceTest.Generator.Expected.custom-base-custom-import.ts")]
        [InlineData(typeof(TestEntities.CustomEmptyBaseClass), "TypeGen.AcceptanceTest.Generator.Expected.custom-empty-base-class.ts")]
        [InlineData(typeof(TestEntities.ExtendedPrimitivesClass), "TypeGen.AcceptanceTest.Generator.Expected.extended-primitives-class.ts")]
        [InlineData(typeof(TestEntities.ExternalDepsClass), "TypeGen.AcceptanceTest.Generator.Expected.external-deps-class.ts")]
        [InlineData(typeof(TestEntities.GenericBaseClass<>), "TypeGen.AcceptanceTest.Generator.Expected.generic-base-class.ts")]
        public async Task TestGenerateSpec(Type type, string expectedLocation)
        {
            var readExpectedTask = EmbededResourceReader.GetEmbeddedResourceAsync(expectedLocation);

            var spec = new AcceptanceTestGenerationSpec();
            var generator = new Gen.Generator();
            var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);

            await generator.GenerateAsync(new[] { spec });
            var expected = (await readExpectedTask).Trim();

            Assert.True(interceptor.GeneratedOutputs.ContainsKey(type));
            Assert.Equal(expected, FromatOutput(interceptor.GeneratedOutputs[type].Content));
        }

        private string FromatOutput(string output) 
            => output
                .Trim()
                .Replace("\n", "") 
                .Replace("\r", "")
                .Replace("\r\n", "");

        private class AcceptanceTestGenerationSpec : GenerationSpec
        {
            public AcceptanceTestGenerationSpec()
            {

                AddClass<TestEntities.CustomBaseClass>().CustomBase("AcmeCustomBase<string>");
                AddInterface<TestEntities.CustomBaseCustomImport>().CustomBase("MB", "./my/base/my-base", "MyBase");
                AddInterface<TestEntities.CustomEmptyBaseClass>().CustomBase();
                AddClass<TestEntities.ExtendedPrimitivesClass>()
                    .Member(x => nameof(x.DateTimeStringField))
                    .Type("string")
                    .Member(x => nameof(x.DateTimeOffsetStringField))
                    .Type("string");
                AddClass<TestEntities.ExternalDepsClass>().Member(nameof(TestEntities.ExternalDepsClass.User)).Ignore();
                AddClass(typeof(TestEntities.GenericBaseClass<>));
                AddClass(typeof(TestEntities.GenericClass<>));
                AddClass(typeof(TestEntities.GenericWithRestrictions<>));
                AddClass<TestEntities.LiteDbEntity>().Member(nameof(TestEntities.LiteDbEntity.MyBsonArray)).Ignore();
                AddInterface<TestEntities.NestedEntity>("./very/nested/directory/").Member(nameof(TestEntities.NestedEntity.OptionalProperty)).Optional();
            }
        }
    }
}
