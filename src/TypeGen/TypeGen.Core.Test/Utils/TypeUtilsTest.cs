using System;
using TypeGen.Core.Test.Business;
using TypeGen.Core.Utils;
using Xunit;

namespace TypeGen.Core.Test.Utils
{
    public class TypeUtilsTest
    {
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
        [InlineData(typeof(TimeSpan), true)]
        [InlineData(typeof(MyClass), false)]
        [InlineData(typeof(int?), false)]
        [InlineData(typeof(DateTime?), false)]
        public void IsTsSimpleType_TypeGiven_DeterminedIfTsSimpleType(Type type, bool expectedResult)
        {
            bool actualResult = TypeUtils.IsTsSimpleType(type);
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
        [InlineData(typeof(TimeSpan), "Date")]
        [InlineData(typeof(MyClass), null)]
        [InlineData(typeof(int?), null)]
        [InlineData(typeof(DateTime?), null)]
        public void GetTsSimpleTypeName_TypeGiven_TsSimpleTypeNameReturned(Type type, string expectedResult)
        {
            string actualResult = TypeUtils.GetTsSimpleTypeName(type);
            Assert.Equal(expectedResult, actualResult);
        }

        private class MyClass
        {
        }
    }
}