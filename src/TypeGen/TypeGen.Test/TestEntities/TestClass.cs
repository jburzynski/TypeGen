using System.Text.RegularExpressions;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Test.TestEntities
{
    [ExportTsClass(OutputDir = "test-classes")]
    internal class TestClass<T, U> : BaseClass<int>
    {
        public string HelloWorld { get; set; }

        [TsType("RegExp", "../acme/regex", "Regex")]
        public Regex Regex { get; set; }

        [TsType("RegExp", "../acme/regex", "Regex")]
        public Regex Regex2 { get; set; }

        [TsType(TsType.Any)]
        public int IntAsAny { get; set; }
    }
}
