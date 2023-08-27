using System.Collections.Generic;
using TypeGen.Core.SpecGeneration;

namespace TypeGen.Cli.TypeResolution;

internal interface IGenerationSpecResolver
{
    IReadOnlyList<GenerationSpec> Resolve(IEnumerable<string> names);
}