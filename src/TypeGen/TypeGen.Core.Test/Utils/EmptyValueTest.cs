using System;
using TypeGen.Core.Test.Business;
using TypeGen.Core.Utils;
using Xunit;

namespace TypeGen.Core.Test.Utils
{
    public class EmptyValueTest
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
        public void ExistsFor_TypeGiven_DeterminedIfEmptyValueExists(Type type, bool expectedResult)
        {
            bool actualResult = EmptyValue.ExistsFor(type);
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Theory]
        [InlineData(typeof(object), false, "{}")]
        [InlineData(typeof(bool), false, "false")]
        [InlineData(typeof(char), false, "\"\"")]
        [InlineData(typeof(char), true, "''")]
        [InlineData(typeof(string), false, "\"\"")]
        [InlineData(typeof(string), true, "''")]
        [InlineData(typeof(Guid), false, "\"\"")]
        [InlineData(typeof(Guid), true, "''")]
        [InlineData(typeof(sbyte), false, "0")]
        [InlineData(typeof(byte), false, "0")]
        [InlineData(typeof(short), false, "0")]
        [InlineData(typeof(ushort), false, "0")]
        [InlineData(typeof(int), false, "0")]
        [InlineData(typeof(uint), false, "0")]
        [InlineData(typeof(long), false, "0")]
        [InlineData(typeof(ulong), false, "0")]
        [InlineData(typeof(float), false, "0")]
        [InlineData(typeof(double), false, "0")]
        [InlineData(typeof(decimal), false, "0")]
        [InlineData(typeof(DateTime), false, "new Date()")]
        [InlineData(typeof(DateTimeOffset), false, "new Date()")]
        [InlineData(typeof(TimeSpan), false, "new Date()")]
        [InlineData(typeof(MyClass), false, null)]
        public void For_TypeGiven_EmptyValueReturned(Type type, bool singleQuotes, string expectedResult)
        {
            string actualResult = EmptyValue.For(type, singleQuotes);
            Assert.Equal(expectedResult, actualResult);
        }
        
        private class MyClass {}
    }
}