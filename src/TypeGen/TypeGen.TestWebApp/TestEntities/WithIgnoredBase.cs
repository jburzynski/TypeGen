using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace Core2WebApp.TestEntities
{
    [ExportTsClass]
    [TsIgnoreBase]
    public class WithIgnoredBase : NotGeneratedBaseClass
    {
        public string SomeProperty { get; set; }
    }
}
