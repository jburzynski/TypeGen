namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class NotReadonlyTrait<TSpecBuilder> : INotReadonlyTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;
    private readonly MemberTrait<TSpecBuilder> _memberTrait;

    public NotReadonlyTrait(TSpecBuilder @this, TypeSpec typeSpec, MemberTrait<TSpecBuilder> memberTrait)
    {
        _this = @this;
        _typeSpec = typeSpec;
        _memberTrait = memberTrait;
    }

    public TSpecBuilder NotReadonly()
    {
        _typeSpec.AddNotReadonlyAttribute(_memberTrait.ActiveMemberName);
        return _this;
    }
}