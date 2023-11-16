namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class ReadonlyTrait<TSpecBuilder> : IReadonlyTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;
    private readonly MemberTrait<TSpecBuilder> _memberTrait;

    public ReadonlyTrait(TSpecBuilder @this, TypeSpec typeSpec, MemberTrait<TSpecBuilder> memberTrait)
    {
        _this = @this;
        _typeSpec = typeSpec;
        _memberTrait = memberTrait;
    }

    public TSpecBuilder Readonly()
    {
        _typeSpec.AddReadonlyAttribute(_memberTrait.ActiveMemberName);
        return _this;
    }
}