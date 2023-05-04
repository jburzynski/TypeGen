using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class DefaultTypeOutputTrait<TSpecBuilder> : IDefaultTypeOutputTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;
    private readonly MemberTrait<TSpecBuilder> _memberTrait;

    public DefaultTypeOutputTrait(TSpecBuilder @this, TypeSpec typeSpec, MemberTrait<TSpecBuilder> memberTrait)
    {
        _this = @this;
        _typeSpec = typeSpec;
        _memberTrait = memberTrait;
    }

    public TSpecBuilder DefaultTypeOutput(string outputDir)
    {
        _typeSpec.AddDefaultTypeOutputAttribute(_memberTrait.ActiveMemberName, outputDir);
        return _this;
    }
}