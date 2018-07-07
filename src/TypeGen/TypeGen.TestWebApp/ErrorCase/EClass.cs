using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApp.TestEntities;

namespace CoreWebApp.ErrorCase
{
    public class EClass : GenericClass<int>
    {
        public const string A = "someUrl";

        [StringLength(250), Required]
        public TestInterface B { get; set; }

        [StringLength(250), Required]
        public string C { get; set; }

        [StringLength(250), Required]
        public string D { get; set; }

        public int E { get; set; }

        [StringLength(250), Required]
        public string F { get; set; }

        [StringLength(1000)]
        public string G { get; set; }

        public bool H { get; set; }

        [StringLength(10)]
        public string I { get; set; }

        public string Link
        {
            get
            {
                return A + F + "/" + D + I;
            }
        }

        public List<FClass> Js { get; set; } = new List<FClass>();
    }
}
