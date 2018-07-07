using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace Core2WebApp.TestEntities
{
    [ExportTsInterface]
    [TsCustomBase("MB", "./my/base/my-base", "MyBase")]
    public class CustomBaseCustomImport : CustomBaseClass
    {
    }
}
