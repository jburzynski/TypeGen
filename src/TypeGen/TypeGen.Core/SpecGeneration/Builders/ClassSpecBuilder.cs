namespace TypeGen.Core.SpecGeneration.Builders;

/// <summary>
/// Builds the class configuration section inside generation spec.
/// </summary>
public class ClassSpecBuilder : ClassSpecBuilderBase<ClassSpecBuilder>
{
    internal ClassSpecBuilder(TypeSpec typeSpec) : base(typeSpec)
    {
    }
}