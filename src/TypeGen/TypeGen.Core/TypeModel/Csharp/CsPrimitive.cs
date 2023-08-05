namespace TypeGen.Core.TypeModel.Csharp;

internal class CsPrimitive : CsType
{
    public CsPrimitive(string fullName, string name, bool isNullable)
        : base(name, isNullable)
    {
        FullName = fullName;
    }
    
    public string FullName { get; }
}