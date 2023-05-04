using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class MemberNameTrait<TSpecBuilder> : IMemberNameTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;
    private readonly MemberTrait<TSpecBuilder> _memberTrait;

    public MemberNameTrait(TSpecBuilder @this, TypeSpec typeSpec, MemberTrait<TSpecBuilder> memberTrait)
    {
        _this = @this;
        _typeSpec = typeSpec;
        _memberTrait = memberTrait;
    }

    public TSpecBuilder MemberName(string name)
    {
        _typeSpec.AddMemberNameAttribute(_memberTrait.ActiveMemberName, name);
        return _this;
    }
}