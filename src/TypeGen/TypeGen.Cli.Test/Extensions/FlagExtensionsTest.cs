using TypeGen.Core;
using Xunit;
using TypeGen.Cli.Extensions;

namespace TypeGen.Cli.Test.Extensions
{
    public class FlagExtensionsTest
    {
        [Theory]
        [InlineData(StrictNullFlags.Regular, "")]
        [InlineData(StrictNullFlags.Null, "null")]
        [InlineData(StrictNullFlags.Undefined, "undefined")]
        [InlineData(StrictNullFlags.Null | StrictNullFlags.Undefined, "null|undefined")]
        public void ToFlagString_FlagsGiven_StringTranslationReturned(StrictNullFlags flags, string expectedResult)
        {
            string actualResult = flags.ToFlagString();
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("", StrictNullFlags.Regular)]
        [InlineData("asdff", StrictNullFlags.Regular)]
        [InlineData("null", StrictNullFlags.Null)]
        [InlineData("undefined", StrictNullFlags.Undefined)]
        [InlineData("null|undefined", StrictNullFlags.Null | StrictNullFlags.Undefined)]
        [InlineData("undefined|null", StrictNullFlags.Null | StrictNullFlags.Undefined)]
        [InlineData("undefined|null|sdfg", StrictNullFlags.Null | StrictNullFlags.Undefined)]
        public void ToStrictNullFlags_StringTranslationGiven_FlagsReturned(string input, StrictNullFlags expectedResult)
        {
            StrictNullFlags actualResult = input.ToStrictNullFlags();
            Assert.Equal(expectedResult, actualResult);
        }
    }
}