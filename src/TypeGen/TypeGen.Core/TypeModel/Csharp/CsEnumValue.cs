namespace TypeGen.Core.TypeModel.Csharp;

internal class CsEnumValue
{
    public CsEnumValue(string name, object underlyingValue)
    {
        Name = name;
        UnderlyingValue = underlyingValue;
    }

    public string Name { get; }
    public object UnderlyingValue { get; }
}