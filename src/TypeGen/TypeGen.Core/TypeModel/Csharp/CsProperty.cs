namespace TypeGen.Core.TypeModel.Csharp;

internal class CsProperty
{
    public CsType Type { get; }
    public bool IsTypeNullable { get; }
    public string Name { get; }
    public object DefaultValue { get; }
}