namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface IIgnoreTrait<TSpecBuilder>
{
    /// <summary>
    /// Marks the selected member as ignored (equivalent of TsIgnoreAttribute).
    /// </summary>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder Ignore();
}