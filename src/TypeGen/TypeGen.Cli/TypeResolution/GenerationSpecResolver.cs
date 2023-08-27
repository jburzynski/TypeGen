using System;
using System.Collections.Generic;
using System.Linq;
using TypeGen.Core.SpecGeneration;

namespace TypeGen.Cli.TypeResolution;

internal class GenerationSpecResolver : IGenerationSpecResolver
{
    private readonly ITypeResolver _typeResolver;

    public GenerationSpecResolver(ITypeResolver typeResolver)
    {
        _typeResolver = typeResolver;
    }

    public IReadOnlyList<GenerationSpec> Resolve(IEnumerable<string> names)
    {
        return names
            .Select(name => _typeResolver.Resolve(name, "GenerationSpec"))
            .Where(t => t != null)
            .Select(t => (GenerationSpec)Activator.CreateInstance(t))
            .ToList();
    }
}