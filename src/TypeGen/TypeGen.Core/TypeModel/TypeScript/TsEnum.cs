using System.Collections.Generic;

namespace TypeGen.Core.TypeModel.TypeScript;

internal class TsEnum : TsType
{
    public TsEnum(string name) : base(name)
    {
    }

    public IReadOnlyCollection<TsEnumValue> Values { get; }
}