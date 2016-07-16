using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Test
{
    [TsInterface]
    public class SomeInterface
    {
        public int SomeInt { get; set; }
        public int SomeIntField;
        public SomeEnum EnumValue;
        public SomeOtherEnum OtherEnumValue { get; set; }
    }
}
