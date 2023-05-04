namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface IStaticTrait<TSpecBuilder>
{
    /// <summary>
    /// Marks the selected member as static (equivalent of TsStaticAttribute).
    /// </summary>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder Static();
}