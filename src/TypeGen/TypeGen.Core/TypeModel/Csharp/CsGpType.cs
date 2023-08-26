#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TypeGen.Core.Extensions;

namespace TypeGen.Core.TypeModel.Csharp;

/// <summary>
/// General purpose C# type. Covers classes, interfaces, structs, records etc.
/// </summary>
internal class CsGpType : CsType
{
    public string FullName { get; init; }
    public IReadOnlyCollection<CsType> GenericTypes { get; init; }
    public CsGpType? Base { get; init; }
    public IReadOnlyCollection<CsGpType> ImplementedInterfaces { get; init; }
    public IReadOnlyCollection<CsField> Fields { get; init; }
    public IReadOnlyCollection<CsProperty> Properties { get; init; }
    public IReadOnlyCollection<Attribute> TgAttributes { get; init; }
    
    public bool IsNonDictionaryEnumerable =>
        FullName != typeof(string).FullName
        && !IsDictionary
        && (ImplementsInterface(typeof(IEnumerable).FullName!) || FullName.StartsWith(typeof(IEnumerable).FullName!));

    public bool IsDictionary =>
        ImplementsInterface(typeof(IDictionary<,>).FullName!)
        || FullName.StartsWith(typeof(IDictionary<,>).FullName!)
        || ImplementsInterface(typeof(IDictionary).FullName!)
        || FullName.StartsWith(typeof(IDictionary).FullName!);
    
    private CsGpType(string name, bool isNullable)
        : base(name, isNullable)
    {
    }

    public static CsGpType Class(string fullName,
        string name,
        bool isNullable,
        IReadOnlyCollection<CsType> genericTypes,
        CsGpType? @base,
        IReadOnlyCollection<CsGpType> implementedInterfaces,
        IReadOnlyCollection<CsField> fields,
        IReadOnlyCollection<CsProperty> properties,
        IReadOnlyCollection<Attribute> tgAttributes)
    {
        return new CsGpType(name, isNullable)
        {
            FullName = fullName,
            GenericTypes = genericTypes,
            Base = @base,
            ImplementedInterfaces = implementedInterfaces,
            Fields = fields,
            Properties = properties,
            TgAttributes = tgAttributes
        };
    }
    
    public static CsGpType Interface(string fullName,
        string name,
        bool isNullable,
        IReadOnlyCollection<CsType> genericTypes,
        CsGpType? @base,
        IReadOnlyCollection<CsProperty> properties,
        IReadOnlyCollection<Attribute> tgAttributes)
    {
        return new CsGpType(name, isNullable)
        {
            FullName = fullName,
            GenericTypes = genericTypes,
            Base = @base,
            Properties = properties,
            TgAttributes = tgAttributes
        };
    }
    
    public static CsGpType Struct(string fullName,
        string name,
        bool isNullable,
        IReadOnlyCollection<CsType> genericTypes,
        IReadOnlyCollection<CsField> fields,
        IReadOnlyCollection<CsProperty> properties,
        IReadOnlyCollection<Attribute> tgAttributes)
    {
        return new CsGpType(name, isNullable)
        {
            FullName = fullName,
            GenericTypes = genericTypes,
            Fields = fields,
            Properties = properties,
            TgAttributes = tgAttributes
        };
    }
    
    private bool ImplementsInterface(string fullName) => ImplementedInterfaces.Contains(x => x.FullName == fullName);
}