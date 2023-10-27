namespace TypeGen.Core.TypeModel.TypeScript;

internal class TsProperty
{
    public string Type { get; }
    public string Name { get; }
    public bool IsOptional { get; }
    public string DefaultValue { get; }
}