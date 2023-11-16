namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class NotNullTrait<TSpecBuilder> : INotNullTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;
    private readonly MemberTrait<TSpecBuilder> _memberTrait;

    public NotNullTrait(TSpecBuilder @this, TypeSpec typeSpec, MemberTrait<TSpecBuilder> memberTrait)
    {
        _this = @this;
        _typeSpec = typeSpec;
        _memberTrait = memberTrait;
    }

    public TSpecBuilder NotNull()
    {
        _typeSpec.AddNotNullAttribute(_memberTrait.ActiveMemberName);
        return _this;
    }
}