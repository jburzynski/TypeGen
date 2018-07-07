using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApp.ErrorCase
{
    public class C
    {
        public int A { get; set; }

        public int B { get; set; }

        public bool D { get; set; }

        public bool E { get; set; }

        public EClass F { get; set; }

        public EClass G { get; set; }

        public List<FClass> Fs { get; set; } = new List<FClass>();

        public Bar Bar { get; set; }
        public int BarId { get; set; }
    }
}
