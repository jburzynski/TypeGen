using System;
using System.Collections.Generic;
using System.Text;
using TypeGen.Core.Extensions;
using Xunit;

namespace TypeGen.Core.Test.Extensions
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("I like hitchhike.", "i", "1", "I l1ke hitchhike.")]
        [InlineData("I like hitchhike.", "k", "1", "I li1e hitchhike.")]
        [InlineData("I like hitchhike.", " ", "_", "I_like hitchhike.")]
        [InlineData("I like hitchhike.", ".", "!", "I like hitchhike!")]
        [InlineData("I like hitchhike.", "ike", "ove", "I love hitchhike.")]
        [InlineData("I like hitchhike.", "I", "You", "You like hitchhike.")]
        [InlineData("I like hitchhike.", "I", "they", "they like hitchhike.")]
        public void ReplaceFirst_Test(string text, string search, string replace, string expectedResult)
        {
            string actualResult = text.ReplaceFirst(search, replace);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("I like a lot of spaces", "ILikeALotOfSpaces")]
        [InlineData("I_like_a_lot_of_spaces", "ILikeALotOfSpaces")]
        [InlineData("I like a lot_of_spaces", "ILikeALotOfSpaces")]
        [InlineData("i like a lot of spaces", "ILikeALotOfSpaces")]
        [InlineData("PascalCaseSingleWord", "PascalCaseSingleWord")]
        [InlineData("camelCaseSingleWord", "CamelCaseSingleWord")]
        public void ToTitleCase_Test(string input, string expectedResult)
        {
            string actualResult = input.ToTitleCase();
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("IDictionary`2", "IDictionary")]
        [InlineData("IList`1", "IList")]
        [InlineData("customtype`1", "customtype")]
        [InlineData("NonGenericType", "NonGenericType")]
        public void RemoveTypeArity_Test(string input, string expectedResult)
        {
            string actualResult = input.RemoveTypeArity();
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Theory]
        [InlineData("IDictionary<string, int>", "IDictionary")]
        [InlineData("IList<int?>", "IList")]
        [InlineData("customtype<customparam>", "customtype")]
        [InlineData("NonGenericType", "NonGenericType")]
        public void RemoveTypeGenericComponent_Test(string input, string expectedResult)
        {
            string actualResult = input.RemoveTypeGenericComponent();
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("string", 0, "string")]
        [InlineData("string | null", 0, "string")]
        [InlineData("string | null", 1, "null")]
        [InlineData("Date | null | undefined", 0, "Date")]
        [InlineData("Date | null | undefined", 1, "null")]
        [InlineData("Date | null | undefined", 2, "undefined")]
        public void GetTsTypeUnion_Test(string input, int index, string expectedResult)
        {
            string actualResult = input.GetTsTypeUnion(index);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
