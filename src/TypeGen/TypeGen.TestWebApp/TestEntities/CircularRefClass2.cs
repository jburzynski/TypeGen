using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace CoreWebApp.TestEntities
{
    public class CircularRefClass2
    {
        public CircularRefClass1 CircularRefClass1 { get; set; }
    }
}
