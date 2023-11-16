using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeGen.Cli.TypeResolution;

internal class ConverterResolver : IConverterResolver
{
    private readonly ITypeResolver _typeResolver;

    public ConverterResolver(ITypeResolver typeResolver)
    {
        _typeResolver = typeResolver;
    }
    
    public IReadOnlyList<T> Resolve<T>(IEnumerable<string> names)
    {
        return names
            .Select(name => _typeResolver.Resolve(name, "Converter", new[] { typeof(T) }))
            .Where(t => t != null)
            .Select(t => (T)Activator.CreateInstance(t))
            .ToList();
    }
}