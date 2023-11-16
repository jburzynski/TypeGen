namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class NotStaticTrait<TSpecBuilder> : INotStaticTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;
    private readonly MemberTrait<TSpecBuilder> _memberTrait;

    public NotStaticTrait(TSpecBuilder @this, TypeSpec typeSpec, MemberTrait<TSpecBuilder> memberTrait)
    {
        _this = @this;
        _typeSpec = typeSpec;
        _memberTrait = memberTrait;
    }

    public TSpecBuilder NotStatic()
    {
        _typeSpec.AddNotStaticAttribute(_memberTrait.ActiveMemberName);
        return _this;
    }
}