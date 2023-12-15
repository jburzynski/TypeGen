namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface IAdditionalPropertiesTrait<TSpecBuilder>
{
    /// <summary>
    /// Adds an additional property to the TypeScript class.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="type">The TypeScript type of the property.</param>
    /// <param name="defaultValue">The TypeScript default value of the property.</param>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder WithAdditionalProperty(string name, string type, string? defaultValue = null);
}
