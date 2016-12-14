using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Test.TestEntities
{
    internal class BaseClass<T>
    {
        public T BaseProperty { get; set; }
    }
}
