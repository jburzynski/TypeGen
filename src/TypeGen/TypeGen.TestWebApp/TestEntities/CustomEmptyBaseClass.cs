using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApp.TestEntities;
using TypeGen.Core.TypeAnnotations;

namespace Core2WebApp.TestEntities
{
    [ExportTsInterface]
    [TsCustomBase]
    public class CustomEmptyBaseClass : BaseClass<string>
    {
        public int SomeProperty { get; set; }
    }
}
