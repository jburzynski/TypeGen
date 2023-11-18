namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class AdditionalPropertiesTrait<TSpecBuilder> : IAdditionalPropertiesTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;

    public AdditionalPropertiesTrait(TSpecBuilder @this, TypeSpec typeSpec)
    {
        _this = @this;
        _typeSpec = typeSpec;
    }

    public TSpecBuilder WithAdditionalProperty(string name, string type, string? defaultValue = null)
    {
        _typeSpec.AddAdditionalClassProperty(name, type, defaultValue);
        return _this;
    }
}