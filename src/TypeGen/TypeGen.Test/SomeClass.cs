using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Test
{
    [ExportTsClass(OutputDir = "./my/classes/../../my/classes/by\\project/")]
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

        public SomeEnum EnumValue { get; set; }

        public SomeOtherEnum OtherEnumValue { get; set; }

        public SomeClass2 Value2 { get; set; }

        public SomeClass3 Value3 { get; set; }

        public int SomeField;

        private int PrivateField; // should not be generated

        private int PrivateProperty { get; set; } // should not be generated
    }
}
