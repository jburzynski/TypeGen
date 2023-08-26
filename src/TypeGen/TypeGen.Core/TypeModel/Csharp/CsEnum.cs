using System;
using System.Collections.Generic;

namespace TypeGen.Core.TypeModel.Csharp;

internal class CsEnum : CsType
{
    public string FullName { get; }
    public IReadOnlyCollection<Attribute> TgAttributes { get; }
    public IReadOnlyCollection<CsEnumValue> Values { get; }
    
    public CsEnum(string fullName,
        string name,
        IReadOnlyCollection<Attribute> tgAttributes,
        IReadOnlyCollection<CsEnumValue> values,
        bool isNullable)
        : base(name, isNullable)
    {
        FullName = fullName;
        Values = values;
        TgAttributes = tgAttributes;
    }

    
}