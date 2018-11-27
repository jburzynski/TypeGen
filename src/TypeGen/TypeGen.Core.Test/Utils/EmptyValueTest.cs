using System;
using TypeGen.Core.Test.Business;
using TypeGen.Core.Utils;
using Xunit;

namespace TypeGen.Core.Test.Utils
{
    public class EmptyValueTest
    {
        [Theory]
        [InlineData("Object", true)]
        [InlineData("boolean", true)]
        [InlineData("string", true)]
        [InlineData("number", true)]
        [InlineData("Date", true)]
        [InlineData("MyClass", false)]
        public void ExistsFor_TypeGiven_DeterminedIfEmptyValueExists(string tsTypeName, bool expectedResult)
        {
            bool actualResult = EmptyValue.ExistsFor(tsTypeName);
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Theory]
        [InlineData("Object", false, "{}")]
        [InlineData("boolean", false, "false")]
        [InlineData("string", false, "\"\"")]
        [InlineData("string", true, "''")]
        [InlineData("number", false, "0")]
        [InlineData("Date", false, "new Date()")]
        [InlineData("MyClass", false, null)]
        public void For_TypeGiven_EmptyValueReturned(string tsTypeName, bool singleQuotes, string expectedResult)
        {
            string actualResult = EmptyValue.For(tsTypeName, singleQuotes);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}