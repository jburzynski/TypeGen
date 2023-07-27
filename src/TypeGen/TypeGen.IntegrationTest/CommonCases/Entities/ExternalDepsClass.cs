using Microsoft.AspNetCore.Identity;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsClass]
    public class ExternalDepsClass
    {
        [TsIgnore]
        public IdentityUser User { get; set; }
    }
}
