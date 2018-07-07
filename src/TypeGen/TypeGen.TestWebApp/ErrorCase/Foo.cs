using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace CoreWebApp.ErrorCase
{
    [ExportTsClass]
    public class Foo
    {
        public FooType Id { get; set; }

        public string A { get; set; }

        public string B { get; set; }

        public int C { get; set; }

        public List<Bar> MyBars { get; set; } = new List<Bar>();
    }
}
