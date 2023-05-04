namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class MemberTrait<TSpecBuilder> : IMemberTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;
    internal string ActiveMemberName { get; private set; }

    public MemberTrait(TSpecBuilder @this, TypeSpec typeSpec)
    {
        _this = @this;
        _typeSpec = typeSpec;
    }

    public TSpecBuilder Member(string memberName)
    {
        ActiveMemberName = memberName;
        _typeSpec.AddMember(ActiveMemberName);
        return _this;
    }
}