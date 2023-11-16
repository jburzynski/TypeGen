namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class NotUndefinedTrait<TSpecBuilder> : INotUndefinedTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;
    private readonly MemberTrait<TSpecBuilder> _memberTrait;

    public NotUndefinedTrait(TSpecBuilder @this, TypeSpec typeSpec, MemberTrait<TSpecBuilder> memberTrait)
    {
        _this = @this;
        _typeSpec = typeSpec;
        _memberTrait = memberTrait;
    }

    public TSpecBuilder NotUndefined()
    {
        _typeSpec.AddNotUndefinedAttribute(_memberTrait.ActiveMemberName);
        return _this;
    }
}