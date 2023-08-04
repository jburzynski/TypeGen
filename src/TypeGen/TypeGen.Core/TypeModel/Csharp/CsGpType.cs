using System;
using System.Collections.Generic;
using System.Linq;
using TypeGen.Core.Extensions;

namespace TypeGen.Core.TypeModel.Csharp;

/// <summary>
/// General purpose C# type. Covers classes, interfaces, structs, records etc.
/// </summary>
internal class CsGpType : CsType
{
    public CsGpType(string name, bool isNullable)
        : base(name, isNullable)
    {
    }
    
    public string FullName { get; }
    public IReadOnlyCollection<CsType> GenericTypes { get; }
    public CsType Base { get; }
    public IReadOnlyCollection<CsGpType> ImplementedInterfaces { get; }
    public IReadOnlyCollection<CsProperty> Fields { get; }
    public IReadOnlyCollection<CsField> Properties { get; }
    public IReadOnlyCollection<Attribute> TgAttributes { get; }
    
    public bool IsNonDictionaryEnumerable =>
        FullName != "System.String"
        && !IsDictionary
        && (ImplementsInterfaceByName("IEnumerable") || FullName.StartsWith("System.Collections.IEnumerable"));

    public bool IsDictionary =>
        ImplementsInterfaceByFullName("System.Collections.Generic.IDictionary`2")
        || FullName.StartsWith("System.Collections.Generic.IDictionary`2")
        || ImplementsInterfaceByFullName("System.Collections.IDictionary")
        || FullName.StartsWith("System.Collections.IDictionary");

    private bool ImplementsInterfaceByName(string name) => ImplementedInterfaces.Contains(x => x.Name == name);
    private bool ImplementsInterfaceByFullName(string fullName) => ImplementedInterfaces.Contains(x => x.FullName == fullName);
}