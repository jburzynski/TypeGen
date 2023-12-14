using System.Collections.Generic;

namespace TypeGen.Core.TypeModel.TypeScript;

internal abstract class TsType
{
    public string Name { get; }
    
    protected TsType(string name)
    {
        Name = name;
    }
}