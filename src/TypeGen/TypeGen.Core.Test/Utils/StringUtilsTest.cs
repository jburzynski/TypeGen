using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TypeGen.Core.Utils;

namespace TypeGen.Core.Test.Utils
{
    public class StringUtilsTest : TestBase
    {
        [Fact]
        public void GetTabText_0Spaces_Empty()
        {
            string tab = StringUtils.GetTabText(0);
            Assert.Equal("", tab);
        }

        [Fact]
        public void GetTabText_1Space_Returned()
        {
            string tab = StringUtils.GetTabText(1);
            Assert.Equal(" ", tab);
        }

        [Fact]
        public void GetTabText_4Spaces_Returned()
        {
            string tab = StringUtils.GetTabText(4);
            Assert.Equal("    ", tab);
        }
    }
}
