using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Types;

namespace TypeGen.Test
{
    [TsClass]
    public class SomeClass
    {
        public string SomeProperty { get; set; }

        [TsDefaultValue("3.14")]
        public double PropertyWithDefaultValue { get; set; }
    }
}
