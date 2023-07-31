using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsClass]
    public class ConstructorChildClass : ConstructorClass
    {
        [TsConstructor]
        public string ChildConstructorArg { get; set; } = "testchild";
    }
}
