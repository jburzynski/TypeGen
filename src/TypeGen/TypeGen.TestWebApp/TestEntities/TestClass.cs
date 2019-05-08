using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsClass(OutputDir = "test-classes")]
    public class TestClass<T, U> : BaseClass<int> where U : BaseClass2<string>
    {
        public string HelloWorld { get; set; }

        [TsType("RegExp", "../acme/regex", "Regex")]
        public Regex Regex { get; set; }

        [TsType("RegExp", "../acme/regex", "Regex")]
        public Regex Regex2 { get; set; }

        [TsType(TsType.Any)]
        public int IntAsAny { get; set; }

        public Dictionary<string, int> DictionaryProperty { get; set; }

        public CircularRefClass1 CircularRefClass1 { get; set; }

        [TsType(TsType.String)]
        public Guid GuidProperty { get; set; }

        public string SampleProperty { get; set; }

        public TestClass<T, U> SelfProperty { get; set; }

        [TsType("SomeOtherClass[][]", "./some-path/some-other-class")]
        public string ArrayTsType { get; set; }

        [TsType("TestClass<string, BaseClass2<string>>", "./some-path/test-class")]
        public int GenericTsType { get; set; }

        [TsType("string?", "./some-path/string")]
        public string UndefinedableTsType { get; set; }

        public BaseClass<string> GenericField;
    }
}
