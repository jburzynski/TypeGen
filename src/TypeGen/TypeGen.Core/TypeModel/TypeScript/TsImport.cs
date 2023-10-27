namespace TypeGen.Core.TypeModel.TypeScript;

internal class TsImport
{
    public string TypeName { get; }
    public string OriginalTypeName { get; }
    public string ImportPath { get; }
    public bool IsDefaultExport { get; }
}