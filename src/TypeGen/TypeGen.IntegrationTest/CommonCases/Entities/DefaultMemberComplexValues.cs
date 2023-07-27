#nullable enable

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    public class DefaultMemberComplexValues
    {
        public int Number { get; set; } = 0;
        public int? NumberNull { get; set; }
        public string String { get; set; } = "default";
        public string? StringNull { get; set; }
    }
}
