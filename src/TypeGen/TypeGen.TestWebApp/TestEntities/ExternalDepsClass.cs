using Microsoft.AspNetCore.Identity;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsClass]
    public class ExternalDepsClass
    {
        [TsIgnore]
        public IdentityUser User { get; set; }
    }
}
