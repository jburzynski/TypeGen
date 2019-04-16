namespace TypeGen.Core.SpecGeneration
{
    /// <summary>
    /// Builds the class configuration section inside generation spec
    /// </summary>
    public class ClassSpecBuilder : CommonClassSpecBuilder<dynamic, ClassSpecBuilder>
    {
        internal ClassSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
    }
}