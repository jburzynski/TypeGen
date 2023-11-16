namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class IgnoreTrait<TSpecBuilder> : IIgnoreTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;
    private readonly MemberTrait<TSpecBuilder> _memberTrait;

    public IgnoreTrait(TSpecBuilder @this, TypeSpec typeSpec, MemberTrait<TSpecBuilder> memberTrait)
    {
        _this = @this;
        _typeSpec = typeSpec;
        _memberTrait = memberTrait;
    }

    public TSpecBuilder Ignore()
    {
        _typeSpec.AddIgnoreAttribute(_memberTrait.ActiveMemberName);
        return _this;
    }
}