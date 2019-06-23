using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using NSubstitute;
using TypeGen.Core.Converters;
using TypeGen.Core.Generator;
using TypeGen.Core.Generator.Services;
using TypeGen.Core.Metadata;
using TypeGen.Core.TypeAnnotations;
using Xunit;

namespace TypeGen.Core.Test.Business
{
    public class TypeServiceTest
    {
        /// <summary>
        /// this needs to be changed to use mocked MetadataReader
        /// </summary>
        private IMetadataReaderFactory _metadataReaderFactory;
        private ITypeService _typeService;

        public TypeServiceTest()
        {
            // this needs to be changed to use mocked MetadataReader
            
            _metadataReaderFactory = Substitute.For<IMetadataReaderFactory>();
            _metadataReaderFactory.GetInstance().Returns(new AttributeMetadataReader());

            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };

            _typeService = new TypeService(_metadataReaderFactory, generatorOptionsProvider);
        }
        
        public class MyClass {}
        public class GenericClass1<T> {}
        public class GenericClass2<T, U> {}
        public class GenericClass3<T, U, V> where U: MyClass {}
        public enum MyEnum {}
        [ExportTsClass] public class TsClass {}
        [ExportTsInterface] public class TsInterface {}
        [ExportTsEnum] public enum TsEnum {}

        [Theory]
        [InlineData(typeof(object), true)]
        [InlineData(typeof(bool), true)]
        [InlineData(typeof(char), true)]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(Guid), true)]
        [InlineData(typeof(sbyte), true)]
        [InlineData(typeof(byte), true)]
        [InlineData(typeof(short), true)]
        [InlineData(typeof(ushort), true)]
        [InlineData(typeof(int), true)]
        [InlineData(typeof(uint), true)]
        [InlineData(typeof(long), true)]
        [InlineData(typeof(ulong), true)]
        [InlineData(typeof(float), true)]
        [InlineData(typeof(double), true)]
        [InlineData(typeof(decimal), true)]
        [InlineData(typeof(DateTime), true)]
        [InlineData(typeof(DateTimeOffset), true)]
        [InlineData(typeof(MyClass), false)]
        [InlineData(typeof(int?), false)]
        [InlineData(typeof(DateTime?), false)]
        public void IsTsSimpleType_TypeGiven_DeterminedIfTsSimpleType(Type type, bool expectedResult)
        {
            bool actualResult = _typeService.IsTsSimpleType(type);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(typeof(object), "Object")]
        [InlineData(typeof(bool), "boolean")]
        [InlineData(typeof(char), "string")]
        [InlineData(typeof(string), "string")]
        [InlineData(typeof(Guid), "string")]
        [InlineData(typeof(sbyte), "number")]
        [InlineData(typeof(byte), "number")]
        [InlineData(typeof(short), "number")]
        [InlineData(typeof(ushort), "number")]
        [InlineData(typeof(int), "number")]
        [InlineData(typeof(uint), "number")]
        [InlineData(typeof(long), "number")]
        [InlineData(typeof(ulong), "number")]
        [InlineData(typeof(float), "number")]
        [InlineData(typeof(double), "number")]
        [InlineData(typeof(decimal), "number")]
        [InlineData(typeof(DateTime), "Date")]
        [InlineData(typeof(DateTimeOffset), "Date")]
        [InlineData(typeof(MyClass), null)]
        [InlineData(typeof(int?), null)]
        [InlineData(typeof(DateTime?), null)]
        public void GetTsSimpleTypeName_TypeGiven_TsSimpleTypeNameReturned(Type type, string expectedResult)
        {
            string actualResult = _typeService.GetTsSimpleTypeName(type);
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Theory]
        [InlineData(typeof(MyClass), true)]
        [InlineData(typeof(MyEnum), false)]
        [InlineData(typeof(TsClass), true)]
        [InlineData(typeof(TsInterface), false)]
        [InlineData(typeof(TsEnum), false)]
        public void IsTsClass_TypeGiven_DeterminedIfTsClass(Type type, bool expectedResult)
        {
            bool actualResult = _typeService.IsTsClass(type);
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Theory]
        [InlineData(typeof(MyClass), false)]
        [InlineData(typeof(MyEnum), false)]
        [InlineData(typeof(TsClass), false)]
        [InlineData(typeof(TsInterface), true)]
        [InlineData(typeof(TsEnum), false)]
        public void IsTsInterface_TypeGiven_DeterminedIfTsInterface(Type type, bool expectedResult)
        {
            bool actualResult = _typeService.IsTsInterface(type);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(GetMemberType_TestData))]
        public void GetMemberType_MemberGiven_MemberTypeReturned(MemberInfo memberInfo, Type expectedResult)
        {
            Type actualResult = _typeService.GetMemberType(memberInfo);
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> GetMemberType_TestData = new[]
        {
            new object[] { typeof(GetMemberType_TestClass).GetField("a"), typeof(string) },
            new object[] { typeof(GetMemberType_TestClass).GetField("b"), typeof(int) },
            new object[] { typeof(GetMemberType_TestClass).GetProperty("C"), typeof(MyClass) },
            new object[] { typeof(GetMemberType_TestClass).GetField("d"), typeof(MyEnum) },
            new object[] { typeof(GetMemberType_TestClass).GetProperty("E"), typeof(MyEnum) },
            new object[] { typeof(GetMemberType_TestClass).GetProperty("F"), typeof(object) },
            new object[] { typeof(GetMemberType_TestClass).GetField("g"), typeof(string[]) },
            new object[] { typeof(GetMemberType_TestClass).GetField("h"), typeof(int[][][]) },
            new object[] { typeof(GetMemberType_TestClass).GetProperty("I"), typeof(IDictionary<int, string>) },
            new object[] { typeof(GetMemberType_TestClass).GetProperty("J"), typeof(IDictionary<int, IList<string[]>>) },
            new object[] { typeof(GetMemberType_TestClass).GetField("k"), typeof(object) }
        };

        public class GetMemberType_TestClass
        {
            public string a;
            public int? b;
            public MyClass C { get; set; }
            public MyEnum d;
            public MyEnum? E { get; set; }
            public dynamic F { get; set; }
            public string[] g;
            public int[][][] h;
            public IDictionary<int, string> I { get; set; }
            public IDictionary<int, IList<string[]>> J { get; set; }
            public object k;
        }

        [Theory]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(object), false)]
        [InlineData(typeof(IDictionary<,>), false)]
        [InlineData(typeof(IDictionary), false)]
        [InlineData(typeof(Dictionary<int, string>), false)]
        [InlineData(typeof(MyClass), false)]
        [InlineData(typeof(MyEnum), false)]
        [InlineData(typeof(int[]), true)]
        [InlineData(typeof(string[]), true)]
        [InlineData(typeof(object[]), true)]
        [InlineData(typeof(int[][][][]), true)]
        [InlineData(typeof(IList<>), true)]
        [InlineData(typeof(IList), true)]
        [InlineData(typeof(IEnumerable), true)]
        [InlineData(typeof(IEnumerable<>), true)]
        [InlineData(typeof(IEnumerable<string>), true)]
        public void IsCollectionType_TypeGiven_DeterminedIfCollectionType(Type type, bool expectedResult)
        {
            bool actualResult = _typeService.IsCollectionType(type);
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Theory]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(object), false)]
        [InlineData(typeof(MyClass), false)]
        [InlineData(typeof(MyEnum), false)]
        [InlineData(typeof(int[]), false)]
        [InlineData(typeof(string[]), false)]
        [InlineData(typeof(object[]), false)]
        [InlineData(typeof(int[][][][]), false)]
        [InlineData(typeof(IList<>), false)]
        [InlineData(typeof(IEnumerable<>), false)]
        [InlineData(typeof(IEnumerable<string>), false)]
        [InlineData(typeof(IDictionary), true)]
        [InlineData(typeof(IDictionary<,>), true)]
        [InlineData(typeof(IDictionary<int, string>), true)]
        [InlineData(typeof(Dictionary<,>), true)]
        [InlineData(typeof(Dictionary<object, int>), true)]
        [InlineData(typeof(HybridDictionary), true)]
        public void IsDictionaryType_TypeGiven_DeterminedIfDictionaryType(Type type, bool expectedResult)
        {
            bool actualResult = _typeService.IsDictionaryType(type);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(typeof(IEnumerable<>), false)]
        [InlineData(typeof(IList<>), false)]
        [InlineData(typeof(IDictionary<,>), false)]
        [InlineData(typeof(Dictionary<int, string>), false)]
        [InlineData(typeof(GenericClass1<>), true)]
        [InlineData(typeof(GenericClass2<,>), true)]
        [InlineData(typeof(GenericClass3<,,>), true)]
        public void IsCustomGenericType_TypeGiven_DeterminedIfCustomGenericType(Type type, bool expectedResult)
        {
            bool actualResult = _typeService.IsCustomGenericType(type);
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Theory]
        [InlineData(typeof(UseDefaultExport_TestClass_NoAttribute), true, true)]
        [InlineData(typeof(UseDefaultExport_TestClass_NoAttribute), false, false)]
        [InlineData(typeof(UseDefaultExport_TestClass_AttributeEnabledTrue), true, true)]
        [InlineData(typeof(UseDefaultExport_TestClass_AttributeEnabledTrue), false, true)]
        [InlineData(typeof(UseDefaultExport_TestClass_AttributeEnabledFalse), true, false)]
        [InlineData(typeof(UseDefaultExport_TestClass_AttributeEnabledFalse), false, false)]
        public void UseDefaultExport_TypeGiven_DeterminedIfDefaultExport(Type type, bool optionsDefaultExport, bool expectedResult)
        {
            // arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions { UseDefaultExport = optionsDefaultExport } };
            _typeService = new TypeService(_metadataReaderFactory, generatorOptionsProvider);
            
            // act
            bool actualResult = _typeService.UseDefaultExport(type);
            
            // assert
            Assert.Equal(expectedResult, actualResult);
        }

        public class UseDefaultExport_TestClass_NoAttribute
        {
        }
        
        [TsDefaultExport]
        public class UseDefaultExport_TestClass_AttributeEnabledTrue
        {
        }
        
        [TsDefaultExport(false)]
        public class UseDefaultExport_TestClass_AttributeEnabledFalse
        {
        }

        [Theory]
        [MemberData(nameof(GetTsTypeName_TestData))]
        public void GetTsTypeName_TypeGiven_TsTypeNameReturned(Type type, TypeNameConverterCollection converters, bool forTypeDeclaration, string expectedResult)
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions { TypeNameConverters = converters } };
            _typeService = new TypeService(_metadataReaderFactory, generatorOptionsProvider);
            
            //act
            string actualResult = _typeService.GetTsTypeName(type, forTypeDeclaration);
            
            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> GetTsTypeName_TestData = new[]
        {
            new object[] { typeof(object), new TypeNameConverterCollection(), false, "Object" },
            new object[] { typeof(object), new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), false, "Object" },
            new object[] { typeof(bool), new TypeNameConverterCollection(), false, "boolean" },
            new object[] { typeof(char), new TypeNameConverterCollection(), false, "string" },
            new object[] { typeof(string), new TypeNameConverterCollection(), false, "string" },
            new object[] { typeof(sbyte), new TypeNameConverterCollection(), false, "number" },
            new object[] { typeof(byte), new TypeNameConverterCollection(), false, "number" },
            new object[] { typeof(short), new TypeNameConverterCollection(), false, "number" },
            new object[] { typeof(ushort), new TypeNameConverterCollection(), false, "number" },
            new object[] { typeof(int), new TypeNameConverterCollection(), false, "number" },
            new object[] { typeof(uint), new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), false, "number" },
            new object[] { typeof(long), new TypeNameConverterCollection(), false, "number" },
            new object[] { typeof(ulong), new TypeNameConverterCollection(), false, "number" },
            new object[] { typeof(float), new TypeNameConverterCollection(), false, "number" },
            new object[] { typeof(double), new TypeNameConverterCollection(), false, "number" },
            new object[] { typeof(decimal), new TypeNameConverterCollection(), false, "number" },
            new object[] { typeof(DateTime), new TypeNameConverterCollection(), false, "Date" },
            new object[] { typeof(MyClass), new TypeNameConverterCollection(), false, "MyClass" },
            new object[] { typeof(MyClass), new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), false, "my-class" },
            new object[] { typeof(MyEnum), new TypeNameConverterCollection(), false, "MyEnum" },
            new object[] { typeof(GenericClass1<>), new TypeNameConverterCollection(), false, "GenericClass1<T>" },
            new object[] { typeof(GenericClass2<int, string>), new TypeNameConverterCollection(), false, "GenericClass2<number, string>" },
            new object[] { typeof(GenericClass3<,,>), new TypeNameConverterCollection(), false, "GenericClass3<T, U, V>" },
            new object[] { typeof(GenericClass3<,,>), new TypeNameConverterCollection(), true, "GenericClass3<T, U extends MyClass, V>" },
            new object[] { typeof(GenericClass3<int, MyClass, string>), new TypeNameConverterCollection(), false, "GenericClass3<number, MyClass, string>" },
            new object[] { typeof(GenericClass3<int, MyClass, string>), new TypeNameConverterCollection(), true, "GenericClass3<number, MyClass, string>" }
        };
        
        [Theory]
        [MemberData(nameof(GetTsTypeName_FromMember_TestData))]
        public void GetTsTypeName_MemberGiven_TsTypeNameReturned(MemberInfo memberInfo, TypeNameConverterCollection converters,
            StrictNullFlags csNullableTranslation, string expectedResult)
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions
            {
                TypeNameConverters = converters,
                CsNullableTranslation = csNullableTranslation
            } };
            _typeService = new TypeService(_metadataReaderFactory, generatorOptionsProvider);
            
            //act
            string actualResult = _typeService.GetTsTypeName(memberInfo);
            
            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        public class GetTsTypeName_TestClass
        {
            public object objectField;
            public bool boolField;
            public char CharProperty { get; set; }
            public string stringField;
            public sbyte SbyteProperty { get; set; }
            public byte byteField;
            public short shortField;
            public ushort ushortField;
            public int intField;
            public uint uintField;
            public long longField;
            public ulong ulongField;
            public float floatField;
            public double doubleField;
            public decimal decimalField;
            public DateTime dateTimeField;
            public MyClass MyClassProperty { get; set; }
            public MyEnum myEnumField;
            public GenericClass1<int> genericClass1Field;
            public GenericClass2<int, string> genericClass2Field;
            public GenericClass3<int, MyClass, string> genericClass3Field;
            public dynamic DynamicProperty { get; set; }
            public int? nullableIntField;
            [TsNull] public int intNullField;
            [TsUndefined] public int intUndefinedField;
            [TsNull, TsUndefined] public int intNullUndefinedField;
            [TsNotNull, TsNull] public byte? nullableByteNotNullNullField;
            [TsNotUndefined, TsUndefined] public int? nullableIntNotUndefinedUndefinedField;
            [TsNull] public int? nullableIntNullField;
            [TsUndefined] public int? nullableIntUndefinedField;
            [TsNull, TsUndefined] public int? nullableIntNullUndefinedField;
            [TsType("CustomType")] public int intCustomTypeField;
            [TsType("Date"), TsNull] public int? nullableIntCustomTypeNullField;
            [TsType("Date"), TsUndefined] public int? nullableIntCustomTypeUndefinedField;
        }

        public static IEnumerable<object[]> GetTsTypeName_FromMember_TestData = new[]
        {
            new object[] { typeof(GetTsTypeName_TestClass).GetField("objectField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "Object" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("objectField"), new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), StrictNullFlags.Regular, "Object" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("boolField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "boolean" },
            new object[] { typeof(GetTsTypeName_TestClass).GetProperty("CharProperty"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "string" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("stringField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "string" },
            new object[] { typeof(GetTsTypeName_TestClass).GetProperty("SbyteProperty"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("byteField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("shortField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("ushortField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("intField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("uintField"), new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), StrictNullFlags.Regular, "number" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("longField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("ulongField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("floatField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("doubleField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("decimalField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("dateTimeField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "Date" },
            new object[] { typeof(GetTsTypeName_TestClass).GetProperty("MyClassProperty"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "MyClass" },
            new object[] { typeof(GetTsTypeName_TestClass).GetProperty("MyClassProperty"), new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), StrictNullFlags.Regular, "my-class" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("myEnumField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "MyEnum" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("genericClass1Field"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "GenericClass1<number>" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("genericClass2Field"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "GenericClass2<number, string>" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("genericClass3Field"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "GenericClass3<number, MyClass, string>" },
            new object[] { typeof(GetTsTypeName_TestClass).GetProperty("DynamicProperty"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "any" },
            new object[] { typeof(GetTsTypeName_TestClass).GetProperty("DynamicProperty"), new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), StrictNullFlags.Regular, "any" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntField"), new TypeNameConverterCollection(), StrictNullFlags.Null, "number | null" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntField"), new TypeNameConverterCollection(), StrictNullFlags.Null | StrictNullFlags.Undefined, "number | null | undefined" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("intNullField"), new TypeNameConverterCollection(), StrictNullFlags.Undefined, "number | null" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("intUndefinedField"), new TypeNameConverterCollection(), StrictNullFlags.Null, "number | undefined" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("intNullUndefinedField"), new TypeNameConverterCollection(), StrictNullFlags.Undefined, "number | null | undefined" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableByteNotNullNullField"), new TypeNameConverterCollection(), StrictNullFlags.Undefined, "number | undefined" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntNotUndefinedUndefinedField"), new TypeNameConverterCollection(), StrictNullFlags.Null, "number | null" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntNullField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number | null" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntNullField"), new TypeNameConverterCollection(), StrictNullFlags.Null, "number | null" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntNullField"), new TypeNameConverterCollection(), StrictNullFlags.Null | StrictNullFlags.Undefined, "number | null | undefined" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntUndefinedField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number | undefined" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntUndefinedField"), new TypeNameConverterCollection(), StrictNullFlags.Undefined, "number | undefined" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntUndefinedField"), new TypeNameConverterCollection(), StrictNullFlags.Null | StrictNullFlags.Undefined, "number | null | undefined" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntNullUndefinedField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "number | null | undefined" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntNullUndefinedField"), new TypeNameConverterCollection(), StrictNullFlags.Null, "number | null | undefined" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntNullUndefinedField"), new TypeNameConverterCollection(), StrictNullFlags.Null | StrictNullFlags.Undefined, "number | null | undefined" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("intCustomTypeField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "CustomType" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntCustomTypeNullField"), new TypeNameConverterCollection(), StrictNullFlags.Undefined, "Date | null | undefined" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntCustomTypeUndefinedField"), new TypeNameConverterCollection(), StrictNullFlags.Null, "Date | null | undefined" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntCustomTypeNullField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "Date | null" },
            new object[] { typeof(GetTsTypeName_TestClass).GetField("nullableIntCustomTypeUndefinedField"), new TypeNameConverterCollection(), StrictNullFlags.Regular, "Date | undefined" },
        };

        [Theory]
        [InlineData(typeof(string), typeof(string))]
        [InlineData(typeof(string[]), typeof(string))]
        [InlineData(typeof(string[][][]), typeof(string))]
        [InlineData(typeof(int[][]), typeof(int))]
        [InlineData(typeof(MyClass), typeof(MyClass))]
        [InlineData(typeof(IEnumerable<MyClass>), typeof(MyClass))]
        [InlineData(typeof(IEnumerable<IEnumerable<MyEnum>>), typeof(MyEnum))]
        [InlineData(typeof(IEnumerable<int[]>), typeof(int))]
        [InlineData(typeof(List<IEnumerable<MyClass[][][]>>), typeof(MyClass))]
        [InlineData(typeof(IEnumerable), typeof(object))]
        [InlineData(typeof(List<IEnumerable>), typeof(object))]
        [InlineData(typeof(IDictionary<string, int>), typeof(IDictionary<string, int>))]
        [InlineData(typeof(HybridDictionary), typeof(HybridDictionary))]
        public void GetFlatType_TypeGiven_FlatTypeReturned(Type type, Type expectedResult)
        {
            Type actualResult = _typeService.GetFlatType(type);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(typeof(string), typeof(string))]
        [InlineData(typeof(int?), typeof(int))]
        [InlineData(typeof(MyClass), typeof(MyClass))]
        [InlineData(typeof(MyEnum), typeof(MyEnum))]
        [InlineData(typeof(MyEnum?), typeof(MyEnum))]
        [InlineData(typeof(GenericClass2<int, string>), typeof(GenericClass2<int, string>))]
        [InlineData(typeof(GenericClass2<,>), typeof(GenericClass2<,>))]
        public void AsNotNullable_TypeGiven_NotNullableTypeReturned(Type type, Type expectedResult)
        {
            Type actualResult = _typeService.StripNullable(type);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(typeof(GetBaseType_TestData1), typeof(MyClass))]
        [InlineData(typeof(GetBaseType_TestData2<int>), typeof(GenericClass1<int>))]
        [InlineData(typeof(GetBaseType_TestData3), typeof(GenericClass3<int, MyClass, string>))]
        [InlineData(typeof(GetBaseType_TestData4<int, string>), typeof(GenericClass3<int, MyClass, string>))]
        public void GetBaseType_TypeGiven_BaseTypeReturned(Type type, Type expectedResult)
        {
            Type actualResult = _typeService.GetBaseType(type);
            Assert.Equal(expectedResult, actualResult);
        }
        
        public class GetBaseType_TestData1 : MyClass {}
        public class GetBaseType_TestData2<T> : GenericClass1<T> {}
        public class GetBaseType_TestData3 : GenericClass3<int, MyClass, string> {}
        public class GetBaseType_TestData4<T, V> : GenericClass3<T, MyClass, V> {}
    }
}
