using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class StringInitializersTrait<TSpecBuilder> : IStringInitializersTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;

    public StringInitializersTrait(TSpecBuilder @this, TypeSpec typeSpec)
    {
        _this = @this;
        _typeSpec = typeSpec;
    }

    public TSpecBuilder StringInitializers(bool enabled = true)
    {
        _typeSpec.AddStringInitializersAttribute(enabled);
        return _this;
    }
}