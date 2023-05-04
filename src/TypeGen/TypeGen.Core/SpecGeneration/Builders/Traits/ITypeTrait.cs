using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface ITypeTrait<TSpecBuilder>
{
    /// <summary>
    /// Specifies custom type for the selected member (equivalent of TsTypeAttribute).
    /// </summary>
    /// <param name="typeName">The TypeScript property type name (or alias).</param>
    /// <param name="importPath">The path of the file to import.</param>
    /// <param name="originalTypeName">The original TypeScript type name, defined in the file under importPath - used only if type alias is specified.</param>
    /// <param name="isDefaultExport">Whether default export is used for the referenced TypeScript type - used only in combination with importPath.</param>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder Type(string typeName, string importPath = null, string originalTypeName = null, bool isDefaultExport = false);
    
    /// <summary>
    /// Specifies custom type for the selected member (equivalent of TsTypeAttribute).
    /// </summary>
    /// <param name="tsType">The TypeScript property's type.</param>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder Type(TsType tsType);
}