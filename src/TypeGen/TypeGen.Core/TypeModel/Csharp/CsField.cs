namespace TypeGen.Core.TypeModel.Csharp;

internal class CsField
{
    public CsField(CsType type, string name, object defaultValue)
    {
        Type = type;
        Name = name;
        DefaultValue = defaultValue;
    }

    public CsType Type { get; }
    public string Name { get; }
    public object DefaultValue { get; }
}