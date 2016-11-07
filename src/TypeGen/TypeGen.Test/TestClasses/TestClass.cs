using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Test.TestClasses
{
    [ExportTsClass]
    internal class TestClass
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
