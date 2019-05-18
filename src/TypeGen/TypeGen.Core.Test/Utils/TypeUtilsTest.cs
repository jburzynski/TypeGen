using System;
using System.Collections.Generic;
using TypeGen.Core.Utils;
using Xunit;

namespace TypeGen.Core.Test.Utils
{
    public class TypeUtilsTest
    {
        [Theory]
        [MemberData(nameof(GetDefaultValue_Test_Data))]
        public void GetDefaultValue_Test(Type type, object expected)
        {
            //act
            object actual = TypeUtils.GetDefaultValue(type);
            
            //assert
            Assert.Equal(expected, actual);
        }

        private class GetDefaultValue_Test_NonGenericClass
        {
        }
        
        private class GetDefaultValue_Test_GenericClass<T1, T2>
        {
        }
        
        public static IEnumerable<object[]> GetDefaultValue_Test_Data = new[]
        {
            new object[] { typeof(object), default(object) },
            new object[] { typeof(bool), default(bool) },
            new object[] { typeof(char), default(char) },
            new object[] { typeof(string), default(string) },
            new object[] { typeof(sbyte), default(sbyte) },
            new object[] { typeof(byte), default(byte) },
            new object[] { typeof(short), default(short) },
            new object[] { typeof(ushort), default(ushort) },
            new object[] { typeof(int), default(int) },
            new object[] { typeof(uint), default(uint) },
            new object[] { typeof(long), default(long) },
            new object[] { typeof(ulong), default(ulong) },
            new object[] { typeof(float), default(float) },
            new object[] { typeof(double), default(double) },
            new object[] { typeof(decimal), default(decimal) },
            new object[] { typeof(GetDefaultValue_Test_NonGenericClass), default(GetDefaultValue_Test_NonGenericClass) },
            new object[] { typeof(GetDefaultValue_Test_GenericClass<,>), null },
            new object[] { typeof(int?), null },
        };
    }
}