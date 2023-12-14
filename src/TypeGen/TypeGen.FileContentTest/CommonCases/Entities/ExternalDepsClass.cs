using Microsoft.AspNetCore.Identity;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsClass]
    public class ExternalDepsClass
    {
        [TsIgnore]
        public IdentityUser User { get; set; }
    }
}
