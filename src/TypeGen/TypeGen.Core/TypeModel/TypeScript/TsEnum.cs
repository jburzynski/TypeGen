using System.Collections.Generic;

namespace TypeGen.Core.TypeModel.TypeScript;

internal class TsEnum : TsType
{
    public IReadOnlyCollection<TsEnumValue> Values { get; }
    
    public TsEnum(string name) : base(name)
    {
    }
}