using System.Collections.Generic;
using System.Linq;
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

    public TSpecBuilder CustomBase(string @base = null, string importPath = null, string originalTypeName = null, bool isDefaultExport = false,
        IEnumerable<ImplementedInterface> implementedInterfaces = null)
    {
        implementedInterfaces = implementedInterfaces ?? Enumerable.Empty<ImplementedInterface>();
        var implementedInterfacesAsAttributeArgument = implementedInterfaces
            .SelectMany(x => new object[] { x.Name, x.ImportPath, x.OriginalTypeName, x.IsDefaultExport })
            .ToArray();
        
        _typeSpec.AddCustomBaseAttribute(@base, importPath, originalTypeName, isDefaultExport, implementedInterfacesAsAttributeArgument);
        return _this;
    }
    
    public TSpecBuilder CustomBase(string @base = null, string importPath = null, string originalTypeName = null, bool isDefaultExport = false,
        params ImplementedInterface[] implementedInterfaces)
        => CustomBase(@base, importPath, originalTypeName, isDefaultExport, (IEnumerable<ImplementedInterface>)implementedInterfaces);
}