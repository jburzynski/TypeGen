using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Test.TestEntities
{
    [ExportTsClass]
    internal class GenericClass<T> : GenericBaseClass<T>
    {
        public T GenericProperty { get; set; }
        public TestEnum EnumProperty { get; set; }
    }
}
