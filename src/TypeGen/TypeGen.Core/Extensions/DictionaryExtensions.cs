using System.Collections.Generic;

namespace TypeGen.Core.Extensions;

public static class DictionaryExtensions
{
    public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default)
        => dict.TryGetValue(key, out var value) ? value : defaultValue;
}