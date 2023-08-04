using System;
using System.Collections.Generic;

namespace TypeGen.Core.TypeModel.Csharp;

internal abstract class CsType
{
    protected CsType(string name, bool isNullable)
    {
        Name = name;
        IsNullable = isNullable;
    }

    public string Name { get; }
    public bool IsNullable { get; }
}