using Microsoft.AspNetCore.Identity;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities.Structs
{
    [ExportTsClass]
    public struct ExternalDepsClass
    {
        [TsIgnore]
        public IdentityUser User { get; set; }
    }
}
