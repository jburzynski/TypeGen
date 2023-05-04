using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class TypeTrait<TSpecBuilder> : ITypeTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;
    private readonly MemberTrait<TSpecBuilder> _memberTrait;

    public TypeTrait(TSpecBuilder @this, TypeSpec typeSpec, MemberTrait<TSpecBuilder> memberTrait)
    {
        _this = @this;
        _typeSpec = typeSpec;
        _memberTrait = memberTrait;
    }

    public TSpecBuilder Type(string typeName, string importPath = null, string originalTypeName = null, bool isDefaultExport = false)
    {
        _typeSpec.AddTypeAttribute(_memberTrait.ActiveMemberName, typeName, importPath, originalTypeName, isDefaultExport);
        return _this;
    }
    
    public TSpecBuilder Type(TsType tsType)
    {
        _typeSpec.AddTypeAttribute(_memberTrait.ActiveMemberName, tsType);
        return _this;
    }
}