using TypeGen.Core;
using Xunit;
using TypeGen.Cli.Extensions;

namespace TypeGen.Cli.Test.Extensions
{
    public class FlagExtensionsTest
    {
        [Theory]
        [InlineData(StrictNullTypeUnionFlags.None, "")]
        [InlineData(StrictNullTypeUnionFlags.Null, "null")]
        [InlineData(StrictNullTypeUnionFlags.Undefined, "undefined")]
        [InlineData(StrictNullTypeUnionFlags.Null | StrictNullTypeUnionFlags.Undefined, "null|undefined")]
        public void ToFlagString_FlagsGiven_StringTranslationReturned(StrictNullTypeUnionFlags typeUnionFlags, string expectedResult)
        {
            string actualResult = typeUnionFlags.ToFlagString();
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("", StrictNullTypeUnionFlags.None)]
        [InlineData("asdff", StrictNullTypeUnionFlags.None)]
        [InlineData("null", StrictNullTypeUnionFlags.Null)]
        [InlineData("undefined", StrictNullTypeUnionFlags.Undefined)]
        [InlineData("null|undefined", StrictNullTypeUnionFlags.Null | StrictNullTypeUnionFlags.Undefined)]
        [InlineData("undefined|null", StrictNullTypeUnionFlags.Null | StrictNullTypeUnionFlags.Undefined)]
        [InlineData("undefined|null|sdfg", StrictNullTypeUnionFlags.Null | StrictNullTypeUnionFlags.Undefined)]
        public void ToStrictNullFlags_StringTranslationGiven_FlagsReturned(string input, StrictNullTypeUnionFlags expectedResult)
        {
            StrictNullTypeUnionFlags actualResult = input.ToStrictNullFlags();
            Assert.Equal(expectedResult, actualResult);
        }
    }
}