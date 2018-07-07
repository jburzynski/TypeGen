using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApp.TestEntities;

namespace CoreWebApp.ErrorCase
{
    public class FClass : GenericClass<int>
    {
        public FClass() { }

        public FClass(string val)
        {
            A = val;
        }

        [Display(Name = "FClass")]
        [StringLength(512)]
        public string A { get; set; }

        public TestInterface B { get; set; }

        public TestInterface C { get; set; }

        public D D { get; set; } // same D as type of Bar.MyDs
        public int? DId { get; set; }
    }
}
