namespace TypeGen.Core.TypeModel.Csharp;

internal class CsProperty
{
    public CsProperty(CsType type, string name, object defaultValue)
    {
        Type = type;
        Name = name;
        DefaultValue = defaultValue;
    }

    public CsType Type { get; }
    public string Name { get; }
    public object DefaultValue { get; }
}