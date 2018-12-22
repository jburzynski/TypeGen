namespace TypeGen.Core.SpecGeneration
{
    public class ClassSpecBuilder : ClassOrInterfaceSpecBuilder<dynamic, ClassSpecBuilder>
    {
        internal ClassSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
    }
}