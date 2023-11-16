namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class DefaultValueTrait<TSpecBuilder> : IDefaultValueTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;
    private readonly MemberTrait<TSpecBuilder> _memberTrait;

    public DefaultValueTrait(TSpecBuilder @this, TypeSpec typeSpec, MemberTrait<TSpecBuilder> memberTrait)
    {
        _this = @this;
        _typeSpec = typeSpec;
        _memberTrait = memberTrait;
    }

    public TSpecBuilder DefaultValue(string defaultValue)
    {
        _typeSpec.AddDefaultValueAttribute(_memberTrait.ActiveMemberName, defaultValue);
        return _this;
    }
}