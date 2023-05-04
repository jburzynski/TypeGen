namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface INotUndefinedTrait<TSpecBuilder>
{
    /// <summary>
    /// Marks the selected member as not undefined (equivalent of TsNotUndefinedAttribute).
    /// </summary>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder NotUndefined();
}