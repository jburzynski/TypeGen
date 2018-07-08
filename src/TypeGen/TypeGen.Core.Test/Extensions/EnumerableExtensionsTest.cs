using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeGen.Core.Extensions;
using Xunit;

namespace TypeGen.Core.Test.Extensions
{
    public class EnumerableExtensionsTest : TestBase
    {
        [Theory]
        [InlineData(1, new object[] { 1, 2, 3 }, true)]
        [InlineData(1, new object[] { 2, 3 }, false)]
        [InlineData("a", new object[] { "a", "b", "c" }, true)]
        [InlineData("a", new object[] { "b", "c" }, false)]
        [InlineData("A", new object[] { "a", "b", "c" }, false)]
        public void In_Test(object element, object[] elements, bool expectedResult)
        {
            bool actualResult = element.In(elements);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(new object[] { 1, 2, 3 }, new object[] { 1, 2, 3 })]
        [InlineData(new object[] { 1, 2, null, 3 }, new object[] { 1, 2, 3 })]
        [InlineData(new object[] { null, 1, 2, null, 3 }, new object[] { 1, 2, 3 })]
        [InlineData(new object[] { null }, new object[] { })]
        [InlineData(new object[] { }, new object[] { })]
        public void WhereNotNull_Test(IEnumerable<object> enumerable, IEnumerable<object> expectedResult)
        {
            object[] actualResult = enumerable.WhereNotNull().ToArray();
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(new object[] { 1, 2, 3 }, 0, true)]
        [InlineData(new object[] { 1, 2, 3 }, 1, true)]
        [InlineData(new object[] { 1, 2, 3 }, 2, true)]
        [InlineData(new object[] { 1, 2, 3 }, 3, false)]
        [InlineData(new object[] { 1, 2, 3 }, 42, false)]
        [InlineData(new object[] { 1, 2, 3 }, -1, false)]
        [InlineData(new object[] { }, 0, false)]
        [InlineData(new object[] { }, 1, false)]
        [InlineData(new object[] { }, -1, false)]
        public void HasIndex_Test(object[] array, int index, bool expectedResult)
        {
            bool actualResult = array.HasIndex(index);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(new object[] { 1, 2, 3 }, false)]
        [InlineData(new object[] { 1 }, false)]
        [InlineData(new object[] { }, true)]
        [InlineData(null, true)]
        public void IsNullOrEmpty_Test(IEnumerable<object> enumerable, bool expectedResult)
        {
            bool actualResult = enumerable.IsNullOrEmpty();
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
