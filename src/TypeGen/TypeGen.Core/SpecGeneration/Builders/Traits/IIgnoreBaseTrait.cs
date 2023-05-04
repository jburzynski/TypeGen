namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface IIgnoreBaseTrait<TSpecBuilder>
{
    /// <summary>
    /// Indicates whether to ignore the base class declaration for the type (equivalent of TsIgnoreBaseAttribute).
    /// </summary>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder IgnoreBase();
}