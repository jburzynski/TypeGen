using System;
using System.Collections.Generic;
using TypeGen.Core.Business;
using TypeGen.Core.TypeAnnotations;
using Xunit;

namespace TypeGen.Core.Test.Business
{
    public class TypeDependencyInfoComparerTest
    {
        [Theory]
        [MemberData(nameof(TypeDependencyInfoComparer_TestData))]
        public void TypeDependencyInfoComparer_Test(object o1, object o2, bool expectedResult)
        {
            var comparer = new TypeDependencyInfoTypeComparer<TypeDependencyInfo>();
            bool actualResult = comparer.Equals((TypeDependencyInfo)o1, (TypeDependencyInfo)o2);
            Assert.Equal(expectedResult, actualResult);
        }
        
        public class Class1 {}
        public class Class2 {}

        public static IEnumerable<object[]> TypeDependencyInfoComparer_TestData = new[]
        {
            new object[] { new TypeDependencyInfo(typeof(Class1)), new TypeDependencyInfo(typeof(Class1)), true },
            new object[] { new TypeDependencyInfo(typeof(Class1), null, true), new TypeDependencyInfo(typeof(Class1), null, false), true },
            new object[] { new TypeDependencyInfo(typeof(Class1), new Attribute[] { new TsIgnoreAttribute() }),
                new TypeDependencyInfo(typeof(Class1), new Attribute[] { new TsIgnoreBaseAttribute() }),
                true },
            new object[] { new TypeDependencyInfo(typeof(Class1), new Attribute[] { new TsIgnoreAttribute() }, false),
                new TypeDependencyInfo(typeof(Class1), new Attribute[] { new TsIgnoreBaseAttribute() }, true),
                true },
            new object[] { new TypeDependencyInfo(typeof(Class1)), new TypeDependencyInfo(typeof(Class2)), false },
            new object[] { new TypeDependencyInfo(typeof(Class1), null, true), new TypeDependencyInfo(typeof(Class2), null, false), false },
            new object[] { new TypeDependencyInfo(typeof(Class2), new Attribute[] { new TsIgnoreAttribute() }),
                new TypeDependencyInfo(typeof(Class1), new Attribute[] { new TsIgnoreBaseAttribute() }),
                false },
            new object[] { new TypeDependencyInfo(typeof(Class2), new Attribute[] { new TsIgnoreAttribute() }, false),
                new TypeDependencyInfo(typeof(Class1), new Attribute[] { new TsIgnoreBaseAttribute() }, true),
                false },
        };
    }
}