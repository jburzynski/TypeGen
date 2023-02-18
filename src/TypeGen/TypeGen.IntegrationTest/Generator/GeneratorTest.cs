using System;
using System.Threading.Tasks;
using TypeGen.Core.SpecGeneration;
using TypeGen.IntegrationTest.Generator.TestingUtils;
using Xunit;
using Gen = TypeGen.Core.Generator;
using TestEntities = TypeGen.TestWebApp.TestEntities;
using Constants = TypeGen.TestWebApp.Constants;
using ErrorCase = TypeGen.TestWebApp.ErrorCase;
using DefaultExport = TypeGen.TestWebApp.DefaultExport;

namespace TypeGen.IntegrationTest.Generator
{
    public class GeneratorTest
    {
        /// <summary>
        /// Tests if a variety of different classes are correctly translated to typescript
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expectedLocation"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(typeof(Constants.FooConstants), "TypeGen.IntegrationTest.Generator.Expected.foo-constants.ts")]
        [InlineData(typeof(TestEntities.CustomBaseCustomImport), "TypeGen.IntegrationTest.Generator.Expected.custom-base-custom-import.ts")]
        [InlineData(typeof(ErrorCase.Bar), "TypeGen.IntegrationTest.Generator.Expected.bar.ts")]
        [InlineData(typeof(TestEntities.BaseClass<>), "TypeGen.IntegrationTest.Generator.Expected.base-class.ts")]
        [InlineData(typeof(TestEntities.BaseClass2<>), "TypeGen.IntegrationTest.Generator.Expected.base-class2.ts")]
        [InlineData(typeof(ErrorCase.C), "TypeGen.IntegrationTest.Generator.Expected.c.ts")]
        [InlineData(typeof(TestEntities.CustomBaseClass), "TypeGen.IntegrationTest.Generator.Expected.custom-base-class.ts")]
        [InlineData(typeof(TestEntities.CustomEmptyBaseClass), "TypeGen.IntegrationTest.Generator.Expected.custom-empty-base-class.ts")]
        [InlineData(typeof(ErrorCase.D), "TypeGen.IntegrationTest.Generator.Expected.d.ts")]
        [InlineData(typeof(TestEntities.DefaultMemberValues), "TypeGen.IntegrationTest.Generator.Expected.default-member-values.ts")]
        [InlineData(typeof(ErrorCase.EClass), "TypeGen.IntegrationTest.Generator.Expected.e-class.ts")]
        [InlineData(typeof(TestEntities.ExtendedPrimitivesClass), "TypeGen.IntegrationTest.Generator.Expected.extended-primitives-class.ts")]
        [InlineData(typeof(TestEntities.ExternalDepsClass), "TypeGen.IntegrationTest.Generator.Expected.external-deps-class.ts")]
        [InlineData(typeof(ErrorCase.FClass), "TypeGen.IntegrationTest.Generator.Expected.f-class.ts")]
        [InlineData(typeof(ErrorCase.FooType), "TypeGen.IntegrationTest.Generator.Expected.foo-type.ts")]
        [InlineData(typeof(ErrorCase.Foo), "TypeGen.IntegrationTest.Generator.Expected.foo.ts")]
        [InlineData(typeof(TestEntities.GenericBaseClass<>), "TypeGen.IntegrationTest.Generator.Expected.generic-base-class.ts")]
        [InlineData(typeof(TestEntities.GenericClass<>), "TypeGen.IntegrationTest.Generator.Expected.generic-class.ts")]
        [InlineData(typeof(TestEntities.GenericWithRestrictions<>), "TypeGen.IntegrationTest.Generator.Expected.generic-with-restrictions.ts")]
        [InlineData(typeof(TestEntities.LiteDbEntity), "TypeGen.IntegrationTest.Generator.Expected.lite-db-entity.ts")]
        [InlineData(typeof(TestEntities.ReadonlyInterface), "TypeGen.IntegrationTest.Generator.Expected.readonly-interface.ts")]
        [InlineData(typeof(TestEntities.StandaloneEnum), "TypeGen.IntegrationTest.Generator.Expected.standalone-enum.ts")]
        [InlineData(typeof(TestEntities.EnumShortValues), "TypeGen.IntegrationTest.Generator.Expected.enum-short-values.ts")]
        [InlineData(typeof(TestEntities.StaticReadonly), "TypeGen.IntegrationTest.Generator.Expected.static-readonly.ts")]
        [InlineData(typeof(TestEntities.StrictNullsClass), "TypeGen.IntegrationTest.Generator.Expected.strict-nulls-class.ts")]
        [InlineData(typeof(TestEntities.TypeUnions), "TypeGen.IntegrationTest.Generator.Expected.type-unions.ts")]
        [InlineData(typeof(TestEntities.WithGenericBaseClassCustomType), "TypeGen.IntegrationTest.Generator.Expected.with-generic-base-class-custom-type.ts")]
        [InlineData(typeof(TestEntities.WithIgnoredBaseAndCustomBase), "TypeGen.IntegrationTest.Generator.Expected.with-ignored-base-and-custom-base.ts")]
        [InlineData(typeof(TestEntities.WithIgnoredBase), "TypeGen.IntegrationTest.Generator.Expected.with-ignored-base.ts")]
        [InlineData(typeof(TestEntities.NoSlashOutputDir), "TypeGen.IntegrationTest.Generator.Expected.no.slash.output.dir.no-slash-output-dir.ts")]
        [InlineData(typeof(TestEntities.BaseClass<>), "TypeGen.IntegrationTest.Generator.Expected.test_classes.base-class.ts")]
        [InlineData(typeof(TestEntities.BaseClass2<>), "TypeGen.IntegrationTest.Generator.Expected.test_classes.base-class2.ts")]
        [InlineData(typeof(TestEntities.CircularRefClass1), "TypeGen.IntegrationTest.Generator.Expected.test_classes.circular-ref-class1.ts")]
        [InlineData(typeof(TestEntities.CircularRefClass2), "TypeGen.IntegrationTest.Generator.Expected.test_classes.circular-ref-class2.ts")]
        [InlineData(typeof(TestEntities.TestClass<,>), "TypeGen.IntegrationTest.Generator.Expected.test_classes.test-class.ts")]
        [InlineData(typeof(TestEntities.TestEnum), "TypeGen.IntegrationTest.Generator.Expected.test_enums.test-enum.ts")]
        [InlineData(typeof(TestEntities.TestInterface), "TypeGen.IntegrationTest.Generator.Expected.test_interfaces.test-interface.ts")]
        [InlineData(typeof(TestEntities.NestedEntity), "TypeGen.IntegrationTest.Generator.Expected.very.nested.directory.nested-entity.ts")]
        [InlineData(typeof(DefaultExport.ClassWithDefaultExport), "TypeGen.IntegrationTest.Generator.Expected.default_export.class-with-default-export.ts")]
        [InlineData(typeof(DefaultExport.GenericClassWithDefaultExport<,>), "TypeGen.IntegrationTest.Generator.Expected.default_export.generic-class-with-default-export.ts")]
        [InlineData(typeof(DefaultExport.ClassWithImports), "TypeGen.IntegrationTest.Generator.Expected.default_export.class-with-imports.ts")]
        [InlineData(typeof(DefaultExport.ClassWithoutDefaultExport), "TypeGen.IntegrationTest.Generator.Expected.default_export.class-without-default-export.ts")]
        [InlineData(typeof(DefaultExport.InterfaceWithDefaultExport), "TypeGen.IntegrationTest.Generator.Expected.default_export.interface-with-default-export.ts")]
        public async Task TestGenerate(Type type, string expectedLocation)
        {
            var readExpectedTask = EmbededResourceReader.GetEmbeddedResourceAsync(expectedLocation);

            var generator = new Gen.Generator();
            var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);

            await generator.GenerateAsync(type.Assembly);
            var expected = (await readExpectedTask).Trim();

            Assert.True(interceptor.GeneratedOutputs.ContainsKey(type));
            Assert.Equal(expected, FormatOutput(interceptor.GeneratedOutputs[type].Content));
        }


        [Theory]
        [InlineData(typeof(TestEntities.CustomBaseClass), "TypeGen.IntegrationTest.Generator.Expected.custom-base-class.ts")]
        [InlineData(typeof(TestEntities.CustomBaseCustomImport), "TypeGen.IntegrationTest.Generator.Expected.custom-base-custom-import.ts")]
        [InlineData(typeof(TestEntities.CustomEmptyBaseClass), "TypeGen.IntegrationTest.Generator.Expected.custom-empty-base-class.ts")]
        [InlineData(typeof(TestEntities.ExtendedPrimitivesClass), "TypeGen.IntegrationTest.Generator.Expected.extended-primitives-class.ts")]
        [InlineData(typeof(TestEntities.ExternalDepsClass), "TypeGen.IntegrationTest.Generator.Expected.external-deps-class.ts")]
        [InlineData(typeof(TestEntities.GenericBaseClass<>), "TypeGen.IntegrationTest.Generator.Expected.generic-base-class.ts")]
        public async Task TestGenerateSpec(Type type, string expectedLocation)
        {
            var readExpectedTask = EmbededResourceReader.GetEmbeddedResourceAsync(expectedLocation);

            var spec = new TestGenerationSpec();
            var generator = new Gen.Generator();
            var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);

            await generator.GenerateAsync(new[] { spec });
            var expected = (await readExpectedTask).Trim();

            Assert.True(interceptor.GeneratedOutputs.ContainsKey(type));
            Assert.Equal(expected, FormatOutput(interceptor.GeneratedOutputs[type].Content));
        }

        [Theory]
        [InlineData(typeof(TestEntities.ConstantsOnly), "TypeGen.IntegrationTest.Generator.Expected.constants-only.ts")]
        public async Task TestConstantsOnlyGenerateSpec(Type type, string expectedLocation)
        {
            var readExpectedTask = EmbededResourceReader.GetEmbeddedResourceAsync(expectedLocation);

            var spec = new TestConstantsOnlyGenerationSpec();
            var generator = new Gen.Generator()
            {
                Options =
                {
                    CsDefaultValuesForConstantsOnly = true
                }
            };
            var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);

            await generator.GenerateAsync(new[] { spec });
            var expected = (await readExpectedTask).Trim();

            Assert.True(interceptor.GeneratedOutputs.ContainsKey(type));
            Assert.Equal(expected, FormatOutput(interceptor.GeneratedOutputs[type].Content));
        }

        private string FormatOutput(string output)
            => output
                .Trim()
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace("\r\n", "");

        private class TestGenerationSpec : GenerationSpec
        {
            public TestGenerationSpec()
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

        private class TestConstantsOnlyGenerationSpec : GenerationSpec
        {
            public TestConstantsOnlyGenerationSpec()
            {
                AddClass<TestEntities.ConstantsOnly>();
            }
        }
    }
}
