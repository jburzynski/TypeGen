namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface IReadonlyTrait<TSpecBuilder>
{
    /// <summary>
    /// Marks the selected member as readonly (equivalent of TsReadonlyAttribute).
    /// </summary>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder Readonly();
}