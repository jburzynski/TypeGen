using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace Core2WebApp.TestEntities
{
    [ExportTsClass]
    [TsCustomBase("AcmeCustomBase<string>")]
    public class CustomBaseClass
    {
        public string SomeProperty { get; set; }
    }
}
