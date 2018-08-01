using System.Collections.Generic;

namespace TypeGen.Cli.Test.Business
{
    public class JsonSerializerTest
    {
        
        
        private class TestClass
        {
            public string stringField;
            public int IntProperty { get; set; }
            public IEnumerable<int> IntEnumerable { get; set; }
            public string[][] nestedStringArray { get; set; }
            public IDictionary<int, string> dictionary;
            public TestDependencyClass dependency;

            public TestClass()
            {
                stringField = "init string";
                IntProperty = 42;
                IntEnumerable = new List<int> { 1, 2, 3 };
                nestedStringArray = new[]
                {
                    new[] { "a", "b", "c" },
                    new[] { "d", "e", "f" },
                };
                dictionary = new Dictionary<int, string> { { 1, "a" }, { 2, "b" } };
                dependency = new TestDependencyClass { stringField = "some dep string", IntProperty = 43 };
            }

            public override bool Equals(object obj)
            {
                if (!(obj is TestClass)) return false;
                var thisObj = (TestClass)obj;

                return stringField.Equals(thisObj.stringField) &&
                       IntProperty.Equals(thisObj.IntProperty) &&
                       IntEnumerable.Equals(thisObj.IntEnumerable) &&
                       nestedStringArray.Equals(thisObj.nestedStringArray) &&
                       dictionary.Equals(thisObj.dictionary) &&
                       dependency.Equals(thisObj.dependency);
            }
        }

        private class TestDependencyClass
        {
            public string stringField;
            public int IntProperty { get; set; }

            public TestDependencyClass()
            {
                stringField = "init dep string";
                IntProperty = 100;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is TestDependencyClass)) return false;
                var thisObj = (TestDependencyClass)obj;

                return stringField.Equals(thisObj.stringField) &&
                       IntProperty.Equals(thisObj.IntProperty);
            }
        }
    }
}