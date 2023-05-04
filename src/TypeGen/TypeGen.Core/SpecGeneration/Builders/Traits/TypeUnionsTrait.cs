using System.Collections.Generic;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class TypeUnionsTrait<TSpecBuilder> : ITypeUnionsTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;
    private readonly MemberTrait<TSpecBuilder> _memberTrait;

    public TypeUnionsTrait(TSpecBuilder @this, TypeSpec typeSpec, MemberTrait<TSpecBuilder> memberTrait)
    {
        _this = @this;
        _typeSpec = typeSpec;
        _memberTrait = memberTrait;
    }

    public TSpecBuilder TypeUnions(IEnumerable<string> typeUnions)
    {
        _typeSpec.AddTypeUnionsAttribute(_memberTrait.ActiveMemberName, typeUnions);
        return _this;
    }
    
    public TSpecBuilder TypeUnions(params string[] typeUnions)
    {
        _typeSpec.AddTypeUnionsAttribute(_memberTrait.ActiveMemberName, typeUnions);
        return _this;
    }
}