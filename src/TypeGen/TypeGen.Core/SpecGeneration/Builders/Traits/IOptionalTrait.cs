namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface IOptionalTrait<TSpecBuilder>
{
    /// <summary>
    /// Marks the selected member as optional (equivalent of TsOptionalAttribute).
    /// </summary>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder Optional();
}