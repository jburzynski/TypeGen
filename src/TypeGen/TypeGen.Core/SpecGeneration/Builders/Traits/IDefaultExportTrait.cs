namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface IDefaultExportTrait<TSpecBuilder>
{
    /// <summary>
    /// Indicates whether to use default export for the generated TypeScript type (equivalent of TsDefaultExportAttribute).
    /// </summary>
    /// <param name="enabled">Whether to enable default export.</param>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder DefaultExport(bool enabled = true);
}