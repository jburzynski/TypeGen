using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class CustomBodyTrait<TSpecBuilder> : ICustomBodyTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;

    public CustomBodyTrait(TSpecBuilder @this, TypeSpec typeSpec)
    {
        _this = @this;
        _typeSpec = typeSpec;
    }

    public TSpecBuilder CustomBody(string body)
    {
        _typeSpec.SetCustomBody(body);
        return _this;
    }
}
