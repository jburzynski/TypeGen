using System;
using System.Collections.Generic;

namespace TypeGen.Core.TypeModel.Csharp;

internal abstract class CsType
{
    protected CsType(string name)
    {
        Name = name;
    }

    public string Name { get; }
}