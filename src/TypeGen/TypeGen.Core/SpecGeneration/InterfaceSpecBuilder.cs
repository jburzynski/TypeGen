namespace TypeGen.Core.SpecGeneration
{
    /// <summary>
    /// Builds the interface configuration section inside generation spec
    /// </summary>
    public class InterfaceSpecBuilder : CommonInterfaceSpecBuilder<dynamic, InterfaceSpecBuilder>
    {
        internal InterfaceSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
    }
}