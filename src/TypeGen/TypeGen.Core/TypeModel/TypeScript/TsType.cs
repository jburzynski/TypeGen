using System.Collections.Generic;

namespace TypeGen.Core.TypeModel.TypeScript;

internal abstract class TsType
{
    protected TsType(string name)
    {
        Name = name;
    }

    public string Name { get; }
}