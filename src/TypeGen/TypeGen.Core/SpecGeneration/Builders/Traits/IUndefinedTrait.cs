namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface IUndefinedTrait<TSpecBuilder>
{
    /// <summary>
    /// Marks the selected member as undefined (equivalent of TsUndefinedAttribute).
    /// </summary>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder Undefined();
}