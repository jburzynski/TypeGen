using Microsoft.AspNetCore.Identity;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities.Structs
{
    [ExportTsClass]
    public struct ExternalDepsClass
    {
        [TsIgnore]
        public IdentityUser User { get; set; }
    }
}
