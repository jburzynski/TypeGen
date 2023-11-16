using Xunit;
using TypeGen.Core.Utils;

namespace TypeGen.Core.Test.Utils
{
    public class StringUtilsTest
    {
        [Theory]
        [InlineData(0, "")]
        [InlineData(1, " ")]
        [InlineData(2, "  ")]
        [InlineData(4, "    ")]
        public void GetTabText_TabLengthGiven_TabTextReturned(int tabLength, string expectedTabText)
        {
            string tab = StringUtils.GetTabText(tabLength);
            Assert.Equal(expectedTabText, tab);
        }
    }
}
