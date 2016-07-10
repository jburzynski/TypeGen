using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Test
{
    [TsClass]
    public class SomeClass
    {
        public string SomeProperty { get; set; }

        [TsDefaultValue("3.14")]
        public double PropertyWithDefaultValue { get; set; }

        public int[] SampleIntArray { get; set; }

        public IEnumerable<string> SampleStringEnumerable { get; set; }

        public List<double> SampleDoubleList { get; set; }

        public IList<string> SampleStringList { get; set; }

        public ICollection<float> SampleFloatCollection { get; set; } 

        public int[][] SampleJaggedIntArray { get; set; }

        public IEnumerable<IEnumerable<string>> SampleJaggedStringEnumerable { get; set; }

        public IEnumerable<List<string>> SampleJaggedStringEnumerableListCombo { get; set; }

        public IEnumerable<float[]> SampleJaggedEnumerableArrayCombo { get; set; }
    }
}
