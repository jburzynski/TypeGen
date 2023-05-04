using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal class DefaultExportTrait<TSpecBuilder> : IDefaultExportTrait<TSpecBuilder>
{
    private readonly TSpecBuilder _this;
    private readonly TypeSpec _typeSpec;

    public DefaultExportTrait(TSpecBuilder @this, TypeSpec typeSpec)
    {
        _this = @this;
        _typeSpec = typeSpec;
    }

    public TSpecBuilder DefaultExport(bool enabled = true)
    {
        _typeSpec.AddDefaultExportAttribute(enabled);
        return _this;
    }
}