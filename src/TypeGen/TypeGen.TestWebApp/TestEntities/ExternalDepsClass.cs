using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TypeGen.Core.TypeAnnotations;

namespace Core2WebApp.TestEntities
{
    [ExportTsClass]
    public class ExternalDepsClass
    {
        [TsIgnore]
        public IdentityUser User { get; set; }
    }
}
