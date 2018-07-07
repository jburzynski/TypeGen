using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace CoreWebApp.TestEntities
{
    public class CircularRefClass1
    {
        public CircularRefClass2 CircularRefClass2 { get; set; }

        [TsType(TsType.String)]
        public Guid GuidProperty { get; set; }
    }
}
