using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class CustomBaseTrait<TSpecBuilder> : ICustomBaseTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;

    public CustomBaseTrait(TSpecBuilder @this, TypeSpec typeSpec)
    {
        _this = @this;
        _typeSpec = typeSpec;
    }

    public TSpecBuilder CustomBase(string @base = null, string importPath = null, string originalTypeName = null, bool isDefaultExport = false)
    {
        _typeSpec.AddCustomBaseAttribute(@base, importPath, originalTypeName, isDefaultExport);
        return _this;
    }
}