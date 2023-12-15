using System;
using System.Threading.Tasks;
using TypeGen.FileContentTest.CommonCases.Entities;
using TypeGen.FileContentTest.CommonCases.Entities.Constants;
using TypeGen.FileContentTest.CommonCases.Entities.ErrorCase;
using TypeGen.FileContentTest.TestingUtils;
using TypeGen.IntegrationTest.CommonCases.Entities;
using Xunit;
using Xunit.Abstractions;
using CustomBaseClass = TypeGen.FileContentTest.CommonCases.Entities.CustomBaseClass;
using CustomBaseCustomImport = TypeGen.FileContentTest.CommonCases.Entities.CustomBaseCustomImport;
using CustomEmptyBaseClass = TypeGen.FileContentTest.CommonCases.Entities.CustomEmptyBaseClass;
using DefaultMemberValues = TypeGen.FileContentTest.CommonCases.Entities.DefaultMemberValues;
using ExtendedPrimitivesClass = TypeGen.FileContentTest.CommonCases.Entities.ExtendedPrimitivesClass;
using ExternalDepsClass = TypeGen.FileContentTest.CommonCases.Entities.ExternalDepsClass;
using ITestInterface = TypeGen.FileContentTest.CommonCases.Entities.ITestInterface;
using LiteDbEntity = TypeGen.FileContentTest.CommonCases.Entities.LiteDbEntity;
using NoSlashOutputDir = TypeGen.FileContentTest.CommonCases.Entities.NoSlashOutputDir;
using ReadonlyInterface = TypeGen.FileContentTest.CommonCases.Entities.ReadonlyInterface;
using StaticReadonly = TypeGen.FileContentTest.CommonCases.Entities.StaticReadonly;
using StrictNullsClass = TypeGen.FileContentTest.CommonCases.Entities.StrictNullsClass;
using TestInterface = TypeGen.FileContentTest.CommonCases.Entities.TestInterface;
using TypeUnions = TypeGen.FileContentTest.CommonCases.Entities.TypeUnions;

namespace TypeGen.FileContentTest.CommonCases
{
    public class CommonCasesGenerationTest : GenerationTestBase
    {
        public CommonCasesGenerationTest(ITestOutputHelper output) : base(output) { }

        /// <summary>
        /// Tests if types are correctly translated to TypeScript.
        /// The tested types contain all major use cases that should be supported.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expectedLocation"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(typeof(FooConstants), "TypeGen.FileContentTest.CommonCases.Expected.foo-constants.ts")]
        [InlineData(typeof(CustomBaseCustomImport), "TypeGen.FileContentTest.CommonCases.Expected.custom-base-custom-import.ts")]
        [InlineData(typeof(Bar), "TypeGen.FileContentTest.CommonCases.Expected.bar.ts")]
        [InlineData(typeof(Entities.BaseClass<>), "TypeGen.FileContentTest.CommonCases.Expected.base-class.ts")]
        [InlineData(typeof(BaseClass2<>), "TypeGen.FileContentTest.CommonCases.Expected.base-class2.ts")]
        [InlineData(typeof(C), "TypeGen.FileContentTest.CommonCases.Expected.c.ts")]
        [InlineData(typeof(ConstructorClass), "TypeGen.FileContentTest.CommonCases.Expected.constructor-class.ts")]
        [InlineData(typeof(ConstructorChildClass), "TypeGen.FileContentTest.CommonCases.Expected.constructor-child-class.ts")]
        [InlineData(typeof(CustomBaseClass), "TypeGen.FileContentTest.CommonCases.Expected.custom-base-class.ts")]
        [InlineData(typeof(CustomEmptyBaseClass), "TypeGen.FileContentTest.CommonCases.Expected.custom-empty-base-class.ts")]
        [InlineData(typeof(D), "TypeGen.FileContentTest.CommonCases.Expected.d.ts")]
        [InlineData(typeof(DefaultMemberValues), "TypeGen.FileContentTest.CommonCases.Expected.default-member-values.ts")]
        [InlineData(typeof(EClass), "TypeGen.FileContentTest.CommonCases.Expected.e-class.ts")]
        [InlineData(typeof(ExtendedPrimitivesClass), "TypeGen.FileContentTest.CommonCases.Expected.extended-primitives-class.ts")]
        [InlineData(typeof(ExternalDepsClass), "TypeGen.FileContentTest.CommonCases.Expected.external-deps-class.ts")]
        [InlineData(typeof(FClass), "TypeGen.FileContentTest.CommonCases.Expected.f-class.ts")]
        [InlineData(typeof(FooType), "TypeGen.FileContentTest.CommonCases.Expected.foo-type.ts")]
        [InlineData(typeof(Foo), "TypeGen.FileContentTest.CommonCases.Expected.foo.ts")]
        [InlineData(typeof(Entities.GenericBaseClass<>), "TypeGen.FileContentTest.CommonCases.Expected.generic-base-class.ts")]
        [InlineData(typeof(GenericClass<>), "TypeGen.FileContentTest.CommonCases.Expected.generic-class.ts")]
        [InlineData(typeof(Entities.GenericWithRestrictions<>), "TypeGen.FileContentTest.CommonCases.Expected.generic-with-restrictions.ts")]
        [InlineData(typeof(ITestInterface), "TypeGen.FileContentTest.CommonCases.Expected.i-test-interface.ts")]
        [InlineData(typeof(LiteDbEntity), "TypeGen.FileContentTest.CommonCases.Expected.lite-db-entity.ts")]
        [InlineData(typeof(ReadonlyInterface), "TypeGen.FileContentTest.CommonCases.Expected.readonly-interface.ts")]
        [InlineData(typeof(StandaloneEnum), "TypeGen.FileContentTest.CommonCases.Expected.standalone-enum.ts")]
        [InlineData(typeof(EnumShortValues), "TypeGen.FileContentTest.CommonCases.Expected.enum-short-values.ts")]
        [InlineData(typeof(EnumAsUnionType), "TypeGen.FileContentTest.CommonCases.Expected.enum-as-union-type.ts")]
        [InlineData(typeof(DictionaryWithEnumKey), "TypeGen.FileContentTest.CommonCases.Expected.dictionary-with-enum-key.ts")]
        [InlineData(typeof(DictionaryStringObjectErrorCase), "TypeGen.FileContentTest.CommonCases.Expected.dictionary-string-object-error-case.ts")]
        [InlineData(typeof(StaticReadonly), "TypeGen.FileContentTest.CommonCases.Expected.static-readonly.ts")]
        [InlineData(typeof(StrictNullsClass), "TypeGen.FileContentTest.CommonCases.Expected.strict-nulls-class.ts")]
        [InlineData(typeof(TypeUnions), "TypeGen.FileContentTest.CommonCases.Expected.type-unions.ts")]
        [InlineData(typeof(WithGenericBaseClassCustomType), "TypeGen.FileContentTest.CommonCases.Expected.with-generic-base-class-custom-type.ts")]
        [InlineData(typeof(WithIgnoredBaseAndCustomBase), "TypeGen.FileContentTest.CommonCases.Expected.with-ignored-base-and-custom-base.ts")]
        [InlineData(typeof(WithIgnoredBase), "TypeGen.FileContentTest.CommonCases.Expected.with-ignored-base.ts")]
        [InlineData(typeof(NoSlashOutputDir), "TypeGen.FileContentTest.CommonCases.Expected.no.slash.output.dir.no-slash-output-dir.ts")]
        [InlineData(typeof(Entities.BaseClass<>), "TypeGen.FileContentTest.CommonCases.Expected.test_classes.base-class.ts")]
        [InlineData(typeof(BaseClass2<>), "TypeGen.FileContentTest.CommonCases.Expected.test_classes.base-class2.ts")]
        [InlineData(typeof(CircularRefClass1), "TypeGen.FileContentTest.CommonCases.Expected.test_classes.circular-ref-class1.ts")]
        [InlineData(typeof(CircularRefClass2), "TypeGen.FileContentTest.CommonCases.Expected.test_classes.circular-ref-class2.ts")]
        [InlineData(typeof(TestClass<,>), "TypeGen.FileContentTest.CommonCases.Expected.test_classes.test-class.ts")]
        [InlineData(typeof(TestEnum), "TypeGen.FileContentTest.CommonCases.Expected.test_enums.test-enum.ts")]
        [InlineData(typeof(TestInterface), "TypeGen.FileContentTest.CommonCases.Expected.test_interfaces.test-interface.ts")]
        [InlineData(typeof(NestedEntity), "TypeGen.FileContentTest.CommonCases.Expected.very.nested.directory.nested-entity.ts")]
        [InlineData(typeof(ArrayOfNullable), "TypeGen.FileContentTest.CommonCases.Expected.array-of-nullable.ts")]
        
        // now do the cases above for structs (when possible)
        
        [InlineData(typeof(Entities.Constants.Structs.FooConstants), "TypeGen.FileContentTest.CommonCases.Expected.foo-constants.ts")]
        [InlineData(typeof(Entities.Structs.CustomBaseCustomImport), "TypeGen.FileContentTest.CommonCases.Expected.custom-base-custom-import.ts")]
        [InlineData(typeof(Entities.Structs.CustomBaseClass), "TypeGen.FileContentTest.CommonCases.Expected.custom-base-class.ts")]
        [InlineData(typeof(Entities.Structs.CustomEmptyBaseClass), "TypeGen.FileContentTest.CommonCases.Expected.custom-empty-base-class.ts")]
        [InlineData(typeof(Entities.Structs.DefaultMemberValues), "TypeGen.FileContentTest.CommonCases.Expected.default-member-values_struct.ts")]
        [InlineData(typeof(Entities.Structs.ExtendedPrimitivesClass), "TypeGen.FileContentTest.CommonCases.Expected.extended-primitives-class.ts")]
        [InlineData(typeof(Entities.Structs.ExternalDepsClass), "TypeGen.FileContentTest.CommonCases.Expected.external-deps-class.ts")]
        [InlineData(typeof(Entities.Structs.GenericBaseClass<>), "TypeGen.FileContentTest.CommonCases.Expected.generic-base-class.ts")]
        [InlineData(typeof(Entities.Structs.GenericWithRestrictions<>), "TypeGen.FileContentTest.CommonCases.Expected.generic-with-restrictions.ts")]
        [InlineData(typeof(Entities.Structs.ITestInterface), "TypeGen.FileContentTest.CommonCases.Expected.i-test-interface.ts")]
        [InlineData(typeof(Entities.Structs.LiteDbEntity), "TypeGen.FileContentTest.CommonCases.Expected.lite-db-entity.ts")]
        [InlineData(typeof(Entities.Structs.ReadonlyInterface), "TypeGen.FileContentTest.CommonCases.Expected.readonly-interface.ts")]
        [InlineData(typeof(Entities.Structs.StaticReadonly), "TypeGen.FileContentTest.CommonCases.Expected.static-readonly.ts")]
        [InlineData(typeof(Entities.Structs.StrictNullsClass), "TypeGen.FileContentTest.CommonCases.Expected.strict-nulls-class.ts")]
        [InlineData(typeof(Entities.Structs.TypeUnions), "TypeGen.FileContentTest.CommonCases.Expected.type-unions.ts")]
        [InlineData(typeof(Entities.Structs.NoSlashOutputDir), "TypeGen.FileContentTest.CommonCases.Expected.no.slash.output.dir.no-slash-output-dir.ts")]
        [InlineData(typeof(Entities.Structs.TestInterface), "TypeGen.FileContentTest.CommonCases.Expected.test_interfaces.test-interface.ts")]
        public async Task TestCommonCases(Type type, string expectedLocation)
        {
            await TestFromAssembly(type, expectedLocation);
        }
    }
}
