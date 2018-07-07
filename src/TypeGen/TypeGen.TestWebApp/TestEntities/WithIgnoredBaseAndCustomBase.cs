using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace Core2WebApp.TestEntities
{
    [ExportTsClass]
    [TsIgnoreBase]
    [TsCustomBase("SomeOtherBase")]
    public class WithIgnoredBaseAndCustomBase : NotGeneratedBaseClass
    {
    }
}
