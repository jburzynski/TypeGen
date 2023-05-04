using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class IgnoreBaseTrait<TSpecBuilder> : IIgnoreBaseTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;

    public IgnoreBaseTrait(TSpecBuilder @this, TypeSpec typeSpec)
    {
        _this = @this;
        _typeSpec = typeSpec;
    }

    public TSpecBuilder IgnoreBase()
    {
        _typeSpec.AddIgnoreBaseAttribute();
        return _this;
    }
}