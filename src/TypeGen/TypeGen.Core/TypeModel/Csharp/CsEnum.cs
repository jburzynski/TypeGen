using System;
using System.Collections.Generic;

namespace TypeGen.Core.TypeModel.Csharp;

internal class CsEnum : CsType
{
    public CsEnum(string fullName,
        string name,
        IReadOnlyCollection<Attribute> attributes,
        IReadOnlyCollection<CsEnumValue> values)
        : base(name)
    {
        FullName = fullName;
        Values = values;
        Attributes = attributes;
    }

    public string FullName { get; }
    public IReadOnlyCollection<Attribute> Attributes { get; }
    public IReadOnlyCollection<CsEnumValue> Values { get; }
}