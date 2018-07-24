using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using TypeGen.Core.Business;
using TypeGen.Core.Converters;
using TypeGen.Core.TypeAnnotations;
using Xunit;

namespace TypeGen.Core.Test.Business
{
    public class TypeServiceTest
    {
        private readonly ITypeService _typeService = new TypeService();

        public class MyClass {}
        public class GenericClass1<T> {}
        public class GenericClass2<T, U> {}
        public class GenericClass3<T, U, V> where U: IEnumerable {}
        public enum MyEnum {}
        [ExportTsClass] public class TsClass {}
        [ExportTsInterface] public class TsInterface {}
        [ExportTsEnum] public enum TsEnum {}

        [Theory]
        [InlineData(typeof(object), true)]
        [InlineData(typeof(bool), true)]
        [InlineData(typeof(char), true)]
        [InlineData(typeof(string), true)]
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

        [Fact]
        public void GetTsExportableMembers_TypeGiven_TsExportableMembersReturned()
        {
            IEnumerable<MemberInfo> expectedResult = new MemberInfo[]
            {
                typeof(GetTsExportableMembers_TestData).GetField("c"),
                typeof(GetTsExportableMembers_TestData).GetField("c1"),
                typeof(GetTsExportableMembers_TestData).GetProperty("C")
            };
            
            IEnumerable<MemberInfo> actualResult = _typeService.GetTsExportableMembers(typeof(GetTsExportableMembers_TestData)).ToArray();
            Assert.Equal(expectedResult, actualResult);
        }
        
        public class GetTsExportableMembers_TestData
        {
            public GetTsExportableMembers_TestData() {}
            private GetTsExportableMembers_TestData(int a) {}
            static GetTsExportableMembers_TestData() {}
            
            private string a;
            private string A { get; set; }
            private static int b;
            private static int B { get; set; }
            public string c;
            public string C { get; set; }
            public readonly int c1;
            private readonly string c2;
            public static int d;
            public static int D { get; set; }
            
            private void e() {}
            private static void E() {}
            private bool f() => true;
            private static bool F() => true;
            public bool g() => true;
            public static bool G() => true;

            private event EventHandler h;
            private static event EventHandler H;
            public event EventHandler i;
            public static event EventHandler I;

            private delegate string j();
            public delegate string k();

            private const int l = 1;
            public const byte L = 2;

            [TsIgnore] public string m;
            [TsIgnore] public int M { get; set; }
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
        [InlineData(typeof(Dictionary<int, string>), false)]
        [InlineData(typeof(MyClass), false)]
        [InlineData(typeof(MyEnum), false)]
        [InlineData(typeof(int[]), true)]
        [InlineData(typeof(string[]), true)]
        [InlineData(typeof(object[]), true)]
        [InlineData(typeof(int[][][][]), true)]
        [InlineData(typeof(IList<>), true)]
        [InlineData(typeof(IList), true)]
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
        [MemberData(nameof(GetTsTypeName_TestData))]
        public void GetTsTypeName_TypeGiven_TsTypeNameReturned(Type type, TypeNameConverterCollection converters, bool forTypeDeclaration, string expectedResult)
        {
            string actualResult = _typeService.GetTsTypeName(type, converters, forTypeDeclaration);
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
            new object[] { typeof(MyEnum), new TypeNameConverterCollection(), false, "MyEnum" }
        };
    }
}
