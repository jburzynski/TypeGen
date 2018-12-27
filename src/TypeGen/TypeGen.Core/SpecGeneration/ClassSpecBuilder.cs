namespace TypeGen.Core.SpecGeneration
{
    /// <summary>
    /// Builds the class configuration section inside generation spec
    /// </summary>
    public class ClassSpecBuilder : ClassOrInterfaceSpecBuilder<dynamic, ClassSpecBuilder>
    {
        internal ClassSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
    }
}