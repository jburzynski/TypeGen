#nullable enable

namespace TypeGen.TestWebApp.TestEntities.Structs
{
    public struct DefaultMemberComplexValues
    {
        public DefaultMemberComplexValues()
        {
            NumberNull = null;
            StringNull = null;
        }

        public int Number { get; set; } = 0;
        public int? NumberNull { get; set; }
        public string String { get; set; } = "default";
        public string? StringNull { get; set; }
    }
}
