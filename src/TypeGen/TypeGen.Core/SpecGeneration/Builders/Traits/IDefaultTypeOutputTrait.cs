namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface IDefaultTypeOutputTrait<TSpecBuilder>
{
    /// <summary>
    /// Specifies the default type output path for the selected member (equivalent of TsDefaultTypeOutputAttribute).
    /// </summary>
    /// <param name="outputDir">The file's default output directory.</param>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder DefaultTypeOutput(string outputDir);
}