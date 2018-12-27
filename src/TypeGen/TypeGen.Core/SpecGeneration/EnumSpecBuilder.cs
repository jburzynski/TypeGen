namespace TypeGen.Core.SpecGeneration
{
    /// <summary>
    /// Builds the enum configuration section inside generation spec
    /// </summary>
    public class EnumSpecBuilder : CommonEnumSpecBuilder<dynamic, EnumSpecBuilder>
    {
        internal EnumSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
    }
}