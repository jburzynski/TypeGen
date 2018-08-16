using System;
using System.Collections.Generic;
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
            Type type = GetType();
            var converter = new PascalCaseToCamelCaseConverter();

            //act
            string actualResultWithType = converter.Convert(input);
            string actualResultWithoutType = converter.Convert(input, type);

            //assert
            Assert.Equal(expectedResult, actualResultWithType);
            Assert.Equal(expectedResult, actualResultWithoutType);
        }

        [Theory]
        [InlineData("SomeName", "some-name")]
        [InlineData("Some", "some")]
        [InlineData("ASomeName", "a-some-name")]
        [InlineData("ASomeAName", "a-some-a-name")]
        public void PascalCaseToKebabCaseConverter_Test(string input, string expectedResult)
        {
            //arrange
            Type type = GetType();
            var converter = new PascalCaseToKebabCaseConverter();

            //act
            string actualResultWithType = converter.Convert(input);
            string actualResultWithoutType = converter.Convert(input, type);

            //assert
            Assert.Equal(expectedResult, actualResultWithType);
            Assert.Equal(expectedResult, actualResultWithoutType);
        }

        [Theory]
        [InlineData("Some_Name", "someName")]
        [InlineData("SOME", "some")]
        [InlineData("A_Some_Name", "aSomeName")]
        [InlineData("A_SOME_A_NAME", "aSomeAName")]
        public void UnderscoreCaseToCamelCaseConverter_Test(string input, string expectedResult)
        {
            //arrange
            Type type = GetType();
            var converter = new UnderscoreCaseToCamelCaseConverter();

            //act
            string actualResultWithType = converter.Convert(input);
            string actualResultWithoutType = converter.Convert(input, type);

            //assert
            Assert.Equal(expectedResult, actualResultWithType);
            Assert.Equal(expectedResult, actualResultWithoutType);
        }

        [Theory]
        [InlineData("Some_Name", "SomeName")]
        [InlineData("SOME", "Some")]
        [InlineData("A_Some_Name", "ASomeName")]
        [InlineData("A_SOME_A_NAME", "ASomeAName")]
        public void UnderscoreCaseToPascalCaseConverter_Test(string input, string expectedResult)
        {
            //arrange
            Type type = GetType();
            var converter = new UnderscoreCaseToPascalCaseConverter();

            //act
            string actualResultWithType = converter.Convert(input);
            string actualResultWithoutType = converter.Convert(input, type);

            //assert
            Assert.Equal(expectedResult, actualResultWithType);
            Assert.Equal(expectedResult, actualResultWithoutType);
        }
    }
}
