using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.Extensions;
using TypeGen.Core.TypeAnnotations;
using Xunit;

namespace TypeGen.Core.Test.Extensions
{
    public class TypeExtensionsTest
    {
        private class PlainClass {}
        [ExportTsClass] private class TsClass {}
        [ExportTsInterface] private class TsInterface {}
        [ExportTsEnum] private enum TsEnum { A }
        
        [Theory]
        [InlineData(typeof(PlainClass), false)]
        [InlineData(typeof(TsClass), true)]
        [InlineData(typeof(TsInterface), true)]
        [InlineData(typeof(TsEnum), true)]
        public void HasExportAttribute_Test(Type type, bool expectedResult)
        {
            bool actualResult = type.HasExportAttribute();
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetExportMarkedTypes_Test()
        {
            IEnumerable<Type> types = new[] { typeof(PlainClass), typeof(TsClass), typeof(TsInterface), typeof(TsEnum), typeof(string), typeof(Type) };
            IEnumerable<Type> expectedResult = new[] { typeof(TsClass), typeof(TsInterface), typeof(TsEnum) };

            IEnumerable<Type> actualResult = types.GetExportMarkedTypes();
            
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void WithoutTsIgnore_Test()
        {
            IEnumerable<MemberInfo> memberInfos = new MemberInfo[]
            {
                typeof(WithoutTsIgnore_TestClass).GetField("a"),
                typeof(WithoutTsIgnore_TestClass).GetProperty("B"),
                typeof(WithoutTsIgnore_TestClass).GetField("c")
            };
            
            IEnumerable<MemberInfo> expectedResult = new MemberInfo[]
            {
                typeof(WithoutTsIgnore_TestClass).GetField("a"),
                typeof(WithoutTsIgnore_TestClass).GetProperty("B")
            };

            IEnumerable<MemberInfo> actualResult = memberInfos.WithoutTsIgnore();
            Assert.Equal(expectedResult, actualResult);
        }

        private class WithoutTsIgnore_TestClass
        {
            public string a;
            public DateTime B { get; set; }
            [TsIgnore] public int c;
        }

        [Fact]
        public void WithMembersFilter_FieldInfoOverload_Test()
        {
            IEnumerable<FieldInfo> fieldInfos = new[]
            {
                typeof(WithMembersFilter_TestClass).GetField("a"),
                typeof(WithMembersFilter_TestClass).GetField("b"),
                typeof(WithMembersFilter_TestClass).GetField("c")
            };
            
            IEnumerable<MemberInfo> expectedResult = new FieldInfo[]
            {
                typeof(WithMembersFilter_TestClass).GetField("a"),
                typeof(WithMembersFilter_TestClass).GetField("c")
            };

            IEnumerable<FieldInfo> actualResult = fieldInfos.WithMembersFilter();
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Fact]
        public void WithMembersFilter_PropertyInfoOverload_Test()
        {
            IEnumerable<PropertyInfo> propertyInfos = new[]
            {
                typeof(WithMembersFilter_TestClass).GetProperty("A"),
                typeof(WithMembersFilter_TestClass).GetProperty("B"),
                typeof(WithMembersFilter_TestClass).GetProperty("C")
            };
            
            IEnumerable<PropertyInfo> expectedResult = new PropertyInfo[]
            {
                typeof(WithMembersFilter_TestClass).GetProperty("A"),
                typeof(WithMembersFilter_TestClass).GetProperty("C")
            };

            IEnumerable<PropertyInfo> actualResult = propertyInfos.WithMembersFilter();
            Assert.Equal(expectedResult, actualResult);
        }

        private class WithMembersFilter_TestClass
        {
            public string a;
            public static DateTime b;
            [TsIgnore] public int c;
            
            public string A { get; set; }
            public static DateTime B { get; set; }
            [TsIgnore] public int C { get; set; }
        }

        [Fact]
        public void GetTypeNames_Test()
        {
            IEnumerable<object> objects = new[] { "a", 2, new List<string>(), new PlainClass(), TsEnum.A, new TsClass(), new object(), DateTime.MinValue };
            IEnumerable<string> expectedResult = new[] { "String", "Int32", "List`1", "PlainClass", "TsEnum", "TsClass", "Object", "DateTime" };

            IEnumerable<string> actualResult = objects.GetTypeNames();
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(typeof(List<>), "IEnumerable", typeof(IEnumerable))]
        [InlineData(typeof(List<string>), "IEnumerable", typeof(IEnumerable))]
        [InlineData(typeof(List<string>), "IEnumerable`1", typeof(IEnumerable<string>))]
        [InlineData(typeof(Dictionary<int, string>), "IEnumerable", typeof(IEnumerable))]
        [InlineData(typeof(Dictionary<int, string>), "IDictionary`2", typeof(IDictionary<int, string>))]
        [InlineData(typeof(Dictionary<int, string>), "asdf", null)]
        [InlineData(typeof(PlainClass), "IEnumerable", null)]
        public void GetInterface_Test(Type type, string interfaceName, Type expectedResult)
        {
            Type actualResult = type.GetInterface(interfaceName);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}