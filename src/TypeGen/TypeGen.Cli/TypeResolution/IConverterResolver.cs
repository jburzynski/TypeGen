using System.Collections.Generic;

namespace TypeGen.Cli.TypeResolution;

internal interface IConverterResolver
{
    IReadOnlyList<T> Resolve<T>(IEnumerable<string> names);
}