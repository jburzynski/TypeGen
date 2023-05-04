namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface IStringInitializersTrait<TSpecBuilder>
{
    /// <summary>
    /// Specifies whether to use TypeScript string initializers for an enum
    /// </summary>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder StringInitializers(bool enabled = true);
}