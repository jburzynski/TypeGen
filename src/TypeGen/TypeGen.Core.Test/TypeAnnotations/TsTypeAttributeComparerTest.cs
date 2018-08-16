using System;
using System.Collections.Generic;
using System.Text;
using TypeGen.Core.TypeAnnotations;
using Xunit;

namespace TypeGen.Core.Test.TypeAnnotations
{
    public class TsTypeAttributeComparerTest
    {
        [Theory]
        [MemberData(nameof(Equals_Test_Data))]
        public void Equals_Test(TsTypeAttribute x, TsTypeAttribute y, bool expectedResult)
        {
            var comparer = new TsTypeAttributeComparer();
            bool actualResult = comparer.Equals(x, y);
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> Equals_Test_Data => new[] {
            new object[] { null, null, true },
            new object[] { new TsTypeAttribute("a", "b", "c"), new TsTypeAttribute("a", "b", "c"), true },
            new object[] { null, new TsTypeAttribute("a", "b", "c"), false },
            new object[] { new TsTypeAttribute("a", "b", "c"), null, false },
            new object[] { new TsTypeAttribute("a", "b", "c"), new TsTypeAttribute("A", "b", "c"), false },
            new object[] { new TsTypeAttribute("a", "B", "c"), new TsTypeAttribute("a", "b", "c"), false },
            new object[] { new TsTypeAttribute("a", "b", "c"), new TsTypeAttribute("a", "b", "C"), false },
            new object[] { new TsTypeAttribute("A", "B", "c"), new TsTypeAttribute("a", "b", "c"), false },
            new object[] { new TsTypeAttribute("a", "b", "c"), new TsTypeAttribute("a", "B", "C"), false },
            new object[] { new TsTypeAttribute("A", "b", "C"), new TsTypeAttribute("a", "b", "c"), false },
            new object[] { new TsTypeAttribute("a", "b", "c"), new TsTypeAttribute("A", "B", "C"), false }
        };
    }
}
