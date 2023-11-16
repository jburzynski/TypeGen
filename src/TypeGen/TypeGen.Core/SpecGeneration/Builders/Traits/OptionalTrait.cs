namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class OptionalTrait<TSpecBuilder> : IOptionalTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;
    private readonly MemberTrait<TSpecBuilder> _memberTrait;

    public OptionalTrait(TSpecBuilder @this, TypeSpec typeSpec, MemberTrait<TSpecBuilder> memberTrait)
    {
        _this = @this;
        _typeSpec = typeSpec;
        _memberTrait = memberTrait;
    }

    public TSpecBuilder Optional()
    {
        _typeSpec.AddOptionalAttribute(_memberTrait.ActiveMemberName);
        return _this;
    }
}