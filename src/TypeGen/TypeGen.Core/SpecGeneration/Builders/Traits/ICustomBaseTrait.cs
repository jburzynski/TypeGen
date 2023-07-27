using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface ICustomBaseTrait<TSpecBuilder>
{
    /// <summary>
    /// Specifies the custom base for the type (equivalent of TsCustomBaseAttribute).
    /// </summary>
    /// <param name="base">The base type name.</param>
    /// <param name="importPath">The path of the custom base type file to import.</param>
    /// <param name="originalTypeName">The original TypeScript base type name.
    /// This property should be used when the specified Base differs from the original base type name defined in the file under ImportPath.
    /// This property should only be used in conjunction with importPath.</param>
    /// <param name="isDefaultExport">Whether default export is used for the referenced TypeScript type - used only in combination with importPath.</param>
    /// <param name="implementedInterfaces">The implemented interfaces.</param>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder CustomBase(string @base = null, string importPath = null, string originalTypeName = null, bool isDefaultExport = false,
        IEnumerable<ImplementedInterface> implementedInterfaces = null);

    /// <summary>
    /// Specifies the custom base for the type (equivalent of TsCustomBaseAttribute).
    /// </summary>
    /// <param name="base">The base type name.</param>
    /// <param name="importPath">The path of the custom base type file to import.</param>
    /// <param name="originalTypeName">The original TypeScript base type name.
    /// This property should be used when the specified Base differs from the original base type name defined in the file under ImportPath.
    /// This property should only be used in conjunction with importPath.</param>
    /// <param name="isDefaultExport">Whether default export is used for the referenced TypeScript type - used only in combination with importPath.</param>
    /// <param name="implementedInterfaces">The implemented interfaces.</param>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder CustomBase(string @base = null, string importPath = null, string originalTypeName = null, bool isDefaultExport = false,
        params ImplementedInterface[] implementedInterfaces);
}