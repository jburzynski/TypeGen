namespace TypeGen.Core.SpecGeneration.Builders
{
    /// <summary>
    /// Base class for all type spec builders
    /// </summary>
    public abstract class SpecBuilderBase
    {
        internal readonly TypeSpec TypeSpec;

        internal SpecBuilderBase(TypeSpec typeSpec)
        {
            TypeSpec = typeSpec;
        }
        
    }
}