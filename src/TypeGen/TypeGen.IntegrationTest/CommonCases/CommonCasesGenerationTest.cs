using System;
using System.Threading.Tasks;
using TypeGen.IntegrationTest.CommonCases.Entities;
using TypeGen.IntegrationTest.CommonCases.Entities.Constants;
using TypeGen.IntegrationTest.CommonCases.Entities.ErrorCase;
using TypeGen.IntegrationTest.CommonCases.Entities.Structs;
using TypeGen.IntegrationTest.DefaultExport.Entities;
using TypeGen.IntegrationTest.IgnoreBaseInterfaces.Entities;
using TypeGen.IntegrationTest.TestingUtils;
using Xunit;
using Gen = TypeGen.Core.Generator;
using CustomBaseClass = TypeGen.IntegrationTest.CommonCases.Entities.CustomBaseClass;
using CustomBaseCustomImport = TypeGen.IntegrationTest.CommonCases.Entities.CustomBaseCustomImport;
using CustomEmptyBaseClass = TypeGen.IntegrationTest.CommonCases.Entities.CustomEmptyBaseClass;
using DefaultMemberValues = TypeGen.IntegrationTest.CommonCases.Entities.DefaultMemberValues;
using ExtendedPrimitivesClass = TypeGen.IntegrationTest.CommonCases.Entities.ExtendedPrimitivesClass;
using ExternalDepsClass = TypeGen.IntegrationTest.CommonCases.Entities.ExternalDepsClass;
using ITestInterface = TypeGen.IntegrationTest.CommonCases.Entities.ITestInterface;
using LiteDbEntity = TypeGen.IntegrationTest.CommonCases.Entities.LiteDbEntity;
using NoSlashOutputDir = TypeGen.IntegrationTest.CommonCases.Entities.NoSlashOutputDir;
using ReadonlyInterface = TypeGen.IntegrationTest.CommonCases.Entities.ReadonlyInterface;
using StaticReadonly = TypeGen.IntegrationTest.CommonCases.Entities.StaticReadonly;
using StrictNullsClass = TypeGen.IntegrationTest.CommonCases.Entities.StrictNullsClass;
using TestInterface = TypeGen.IntegrationTest.CommonCases.Entities.TestInterface;
using TypeUnions = TypeGen.IntegrationTest.CommonCases.Entities.TypeUnions;

namespace TypeGen.IntegrationTest.CommonCases
{
    public class CommonCasesGenerationTest : GenerationTestBase
    {
        /// <summary>
        /// Tests if types are correctly translated to TypeScript.
        /// The tested types contain all major use cases that should be supported.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expectedLocation"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(typeof(FooConstants), "TypeGen.IntegrationTest.CommonCases.Expected.foo-constants.ts")]
        [InlineData(typeof(CustomBaseCustomImport), "TypeGen.IntegrationTest.CommonCases.Expected.custom-base-custom-import.ts")]
        [InlineData(typeof(Bar), "TypeGen.IntegrationTest.CommonCases.Expected.bar.ts")]
        [InlineData(typeof(Entities.BaseClass<>), "TypeGen.IntegrationTest.CommonCases.Expected.base-class.ts")]
        [InlineData(typeof(BaseClass2<>), "TypeGen.IntegrationTest.CommonCases.Expected.base-class2.ts")]
        [InlineData(typeof(C), "TypeGen.IntegrationTest.CommonCases.Expected.c.ts")]
        [InlineData(typeof(CustomBaseClass), "TypeGen.IntegrationTest.CommonCases.Expected.custom-base-class.ts")]
        [InlineData(typeof(CustomEmptyBaseClass), "TypeGen.IntegrationTest.CommonCases.Expected.custom-empty-base-class.ts")]
        [InlineData(typeof(D), "TypeGen.IntegrationTest.CommonCases.Expected.d.ts")]
        [InlineData(typeof(DefaultMemberValues), "TypeGen.IntegrationTest.CommonCases.Expected.default-member-values.ts")]
        [InlineData(typeof(EClass), "TypeGen.IntegrationTest.CommonCases.Expected.e-class.ts")]
        [InlineData(typeof(ExtendedPrimitivesClass), "TypeGen.IntegrationTest.CommonCases.Expected.extended-primitives-class.ts")]
        [InlineData(typeof(ExternalDepsClass), "TypeGen.IntegrationTest.CommonCases.Expected.external-deps-class.ts")]
        [InlineData(typeof(FClass), "TypeGen.IntegrationTest.CommonCases.Expected.f-class.ts")]
        [InlineData(typeof(FooType), "TypeGen.IntegrationTest.CommonCases.Expected.foo-type.ts")]
        [InlineData(typeof(Foo), "TypeGen.IntegrationTest.CommonCases.Expected.foo.ts")]
        [InlineData(typeof(Entities.GenericBaseClass<>), "TypeGen.IntegrationTest.CommonCases.Expected.generic-base-class.ts")]
        [InlineData(typeof(GenericClass<>), "TypeGen.IntegrationTest.CommonCases.Expected.generic-class.ts")]
        [InlineData(typeof(Entities.GenericWithRestrictions<>), "TypeGen.IntegrationTest.CommonCases.Expected.generic-with-restrictions.ts")]
        [InlineData(typeof(ITestInterface), "TypeGen.IntegrationTest.CommonCases.Expected.i-test-interface.ts")]
        [InlineData(typeof(LiteDbEntity), "TypeGen.IntegrationTest.CommonCases.Expected.lite-db-entity.ts")]
        [InlineData(typeof(ReadonlyInterface), "TypeGen.IntegrationTest.CommonCases.Expected.readonly-interface.ts")]
        [InlineData(typeof(StandaloneEnum), "TypeGen.IntegrationTest.CommonCases.Expected.standalone-enum.ts")]
        [InlineData(typeof(EnumShortValues), "TypeGen.IntegrationTest.CommonCases.Expected.enum-short-values.ts")]
        [InlineData(typeof(EnumAsUnionType), "TypeGen.IntegrationTest.CommonCases.Expected.enum-as-union-type.ts")]
        [InlineData(typeof(DictionaryWithEnumKey), "TypeGen.IntegrationTest.CommonCases.Expected.dictionary-with-enum-key.ts")]
        [InlineData(typeof(DictionaryStringObjectErrorCase), "TypeGen.IntegrationTest.CommonCases.Expected.dictionary-string-object-error-case.ts")]
        [InlineData(typeof(StaticReadonly), "TypeGen.IntegrationTest.CommonCases.Expected.static-readonly.ts")]
        [InlineData(typeof(StrictNullsClass), "TypeGen.IntegrationTest.CommonCases.Expected.strict-nulls-class.ts")]
        [InlineData(typeof(TypeUnions), "TypeGen.IntegrationTest.CommonCases.Expected.type-unions.ts")]
        [InlineData(typeof(WithGenericBaseClassCustomType), "TypeGen.IntegrationTest.CommonCases.Expected.with-generic-base-class-custom-type.ts")]
        [InlineData(typeof(WithIgnoredBaseAndCustomBase), "TypeGen.IntegrationTest.CommonCases.Expected.with-ignored-base-and-custom-base.ts")]
        [InlineData(typeof(WithIgnoredBase), "TypeGen.IntegrationTest.CommonCases.Expected.with-ignored-base.ts")]
        [InlineData(typeof(NoSlashOutputDir), "TypeGen.IntegrationTest.CommonCases.Expected.no.slash.output.dir.no-slash-output-dir.ts")]
        [InlineData(typeof(Entities.BaseClass<>), "TypeGen.IntegrationTest.CommonCases.Expected.test_classes.base-class.ts")]
        [InlineData(typeof(BaseClass2<>), "TypeGen.IntegrationTest.CommonCases.Expected.test_classes.base-class2.ts")]
        [InlineData(typeof(CircularRefClass1), "TypeGen.IntegrationTest.CommonCases.Expected.test_classes.circular-ref-class1.ts")]
        [InlineData(typeof(CircularRefClass2), "TypeGen.IntegrationTest.CommonCases.Expected.test_classes.circular-ref-class2.ts")]
        [InlineData(typeof(TestClass<,>), "TypeGen.IntegrationTest.CommonCases.Expected.test_classes.test-class.ts")]
        [InlineData(typeof(TestEnum), "TypeGen.IntegrationTest.CommonCases.Expected.test_enums.test-enum.ts")]
        [InlineData(typeof(TestInterface), "TypeGen.IntegrationTest.CommonCases.Expected.test_interfaces.test-interface.ts")]
        [InlineData(typeof(NestedEntity), "TypeGen.IntegrationTest.CommonCases.Expected.very.nested.directory.nested-entity.ts")]
        [InlineData(typeof(ArrayOfNullable), "TypeGen.IntegrationTest.CommonCases.Expected.array-of-nullable.ts")]
        
        // now do the cases above for structs (when possible)
        
        [InlineData(typeof(Entities.Constants.Structs.FooConstants), "TypeGen.IntegrationTest.CommonCases.Expected.foo-constants.ts")]
        [InlineData(typeof(Entities.Structs.CustomBaseCustomImport), "TypeGen.IntegrationTest.CommonCases.Expected.custom-base-custom-import.ts")]
        [InlineData(typeof(Entities.Structs.CustomBaseClass), "TypeGen.IntegrationTest.CommonCases.Expected.custom-base-class.ts")]
        [InlineData(typeof(Entities.Structs.CustomEmptyBaseClass), "TypeGen.IntegrationTest.CommonCases.Expected.custom-empty-base-class.ts")]
        [InlineData(typeof(Entities.Structs.DefaultMemberValues), "TypeGen.IntegrationTest.CommonCases.Expected.default-member-values_struct.ts")]
        [InlineData(typeof(Entities.Structs.ExtendedPrimitivesClass), "TypeGen.IntegrationTest.CommonCases.Expected.extended-primitives-class.ts")]
        [InlineData(typeof(Entities.Structs.ExternalDepsClass), "TypeGen.IntegrationTest.CommonCases.Expected.external-deps-class.ts")]
        [InlineData(typeof(Entities.Structs.GenericBaseClass<>), "TypeGen.IntegrationTest.CommonCases.Expected.generic-base-class.ts")]
        [InlineData(typeof(Entities.Structs.GenericWithRestrictions<>), "TypeGen.IntegrationTest.CommonCases.Expected.generic-with-restrictions.ts")]
        [InlineData(typeof(Entities.Structs.ITestInterface), "TypeGen.IntegrationTest.CommonCases.Expected.i-test-interface.ts")]
        [InlineData(typeof(Entities.Structs.LiteDbEntity), "TypeGen.IntegrationTest.CommonCases.Expected.lite-db-entity.ts")]
        [InlineData(typeof(Entities.Structs.ReadonlyInterface), "TypeGen.IntegrationTest.CommonCases.Expected.readonly-interface.ts")]
        [InlineData(typeof(Entities.Structs.StaticReadonly), "TypeGen.IntegrationTest.CommonCases.Expected.static-readonly.ts")]
        [InlineData(typeof(Entities.Structs.StrictNullsClass), "TypeGen.IntegrationTest.CommonCases.Expected.strict-nulls-class.ts")]
        [InlineData(typeof(Entities.Structs.TypeUnions), "TypeGen.IntegrationTest.CommonCases.Expected.type-unions.ts")]
        [InlineData(typeof(Entities.Structs.NoSlashOutputDir), "TypeGen.IntegrationTest.CommonCases.Expected.no.slash.output.dir.no-slash-output-dir.ts")]
        [InlineData(typeof(Entities.Structs.TestInterface), "TypeGen.IntegrationTest.CommonCases.Expected.test_interfaces.test-interface.ts")]
        public async Task TestCommonCases(Type type, string expectedLocation)
        {
            await TestFromAssembly(type, expectedLocation);
        }
    }
}
