using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsClass]
    public class ConstructorClass
    {
        [TsConstructor]
        public int ConstructorArgWithDefault { get; set; } = 4;

        [TsConstructor]
        [TsDefaultValue("5")]
        public int ConstructorArgWithAttributeDefault { get; set; }

        [TsConstructor]
        public int ConstructorArgWithNoDefault { get; set; }

        public string NonConstructorArg { get; set; } = "test";
    }
}
