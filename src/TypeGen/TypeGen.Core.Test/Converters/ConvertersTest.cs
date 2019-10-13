using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TypeGen.Core.Converters;
using Xunit;

namespace TypeGen.Core.Test.Converters
{
    public class ConvertersTest
    {
        [Theory]
        [InlineData("SomeName", "someName")]
        [InlineData("Some", "some")]
        [InlineData("ASomeName", "aSomeName")]
        [InlineData("ASomeAName", "aSomeAName")]
        public void PascalCaseToCamelCaseConverter_Test(string input, string expectedResult)
        {
            //arrange
            var converter = new PascalCaseToCamelCaseConverter();

            //act
            string actualResultMember = converter.Convert(input, (MemberInfo)null);
            string actualResultType = converter.Convert(input, (Type)null);

            //assert
            Assert.Equal(expectedResult, actualResultMember);
            Assert.Equal(expectedResult, actualResultType);
        }

        [Theory]
        [InlineData("SomeName", "some-name")]
        [InlineData("Some", "some")]
        [InlineData("ASomeName", "a-some-name")]
        [InlineData("ASomeAName", "a-some-a-name")]
        public void PascalCaseToKebabCaseConverter_Test(string input, string expectedResult)
        {
            //arrange
            var converter = new PascalCaseToKebabCaseConverter();

            //act
            string actualResultMember = converter.Convert(input, (MemberInfo)null);
            string actualResultType = converter.Convert(input, (Type)null);

            //assert
            Assert.Equal(expectedResult, actualResultMember);
            Assert.Equal(expectedResult, actualResultType);
        }

        [Theory]
        [InlineData("Some_Name", "someName")]
        [InlineData("SOME", "some")]
        [InlineData("SomeName", "someName")]
        [InlineData("someName", "someName")]
        [InlineData("S", "s")]
        [InlineData("A_Some_Name", "aSomeName")]
        [InlineData("A_SOME_A_NAME", "aSomeAName")]
        public void UnderscoreCaseToCamelCaseConverter_Test(string input, string expectedResult)
        {
            //arrange
            var converter = new UnderscoreCaseToCamelCaseConverter();

            //act
            string actualResultMember = converter.Convert(input, (MemberInfo)null);
            string actualResultType = converter.Convert(input, (Type)null);

            //assert
            Assert.Equal(expectedResult, actualResultMember);
            Assert.Equal(expectedResult, actualResultType);
        }

        [Theory]
        [InlineData("Some_Name", "SomeName")]
        [InlineData("SOME", "Some")]
        [InlineData("SomeName", "SomeName")]
        [InlineData("someName", "SomeName")]
        [InlineData("S", "S")]
        [InlineData("A_Some_Name", "ASomeName")]
        [InlineData("A_SOME_A_NAME", "ASomeAName")]
        public void UnderscoreCaseToPascalCaseConverter_Test(string input, string expectedResult)
        {
            //arrange
            var converter = new UnderscoreCaseToPascalCaseConverter();

            //act
            string actualResultMember = converter.Convert(input, (MemberInfo)null);
            string actualResultType = converter.Convert(input, (Type)null);

            //assert
            Assert.Equal(expectedResult, actualResultMember);
            Assert.Equal(expectedResult, actualResultType);
        }
    }
}
