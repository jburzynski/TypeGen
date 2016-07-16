using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Test
{
    [TsClass(OutputDir = "my/classes/by/project")]
    public class SomeClass3
    {
        public object SomeObject { get; set; }
    }
}
