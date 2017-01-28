using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Test.TestEntities
{
    [ExportTsInterface(OutputDir = "test-interfaces")]
    internal class TestInterface
    {
        public string Property { get; set; }
    }
}
