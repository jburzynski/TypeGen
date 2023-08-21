using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class CustomHeaderTrait<TSpecBuilder> : ICustomHeaderTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;

    public CustomHeaderTrait(TSpecBuilder @this, TypeSpec typeSpec)
    {
        _this = @this;
        _typeSpec = typeSpec;
    }

    public TSpecBuilder CustomHeader(string header)
    {
        _typeSpec.SetCustomHeader(header);
        return _this;
    }
}
