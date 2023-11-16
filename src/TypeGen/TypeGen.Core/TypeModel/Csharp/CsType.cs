namespace TypeGen.Core.TypeModel.Csharp;

internal abstract class CsType
{
    public string Name { get; }
    public bool IsNullable { get; }
    
    protected CsType(string name, bool isNullable)
    {
        Name = name;
        IsNullable = isNullable;
    }
}