using System;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class MemberGenericTrait<TType, TSpecBuilder> : IMemberGenericTrait<TType, TSpecBuilder>
{
    private readonly TType _tTypeInstance;
    private readonly MemberTrait<TSpecBuilder> _memberTrait;

    public MemberGenericTrait(TSpecBuilder @this, TypeSpec typeSpec, MemberTrait<TSpecBuilder> memberTrait)
    {
        _memberTrait = memberTrait;
        _tTypeInstance = default;
    }

    public TSpecBuilder Member(Func<TType, string> memberNameFunc)
        => _memberTrait.Member(memberNameFunc(_tTypeInstance));
}