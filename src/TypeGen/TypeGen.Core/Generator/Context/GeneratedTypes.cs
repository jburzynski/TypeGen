using System;
using System.Collections.Generic;
using System.Linq;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Generator.Context;

internal class GeneratedTypes
{
    /// <summary>
    /// Types that have already been generated in the current session.
    /// </summary>
    private readonly IList<Type> _allGeneratedTypes = new List<Type>();

    /// <summary>
    /// Types that have already been generated for the currently generated type.
    /// </summary>
    private IList<Type> _currentTypeGeneratedTypes;
    
    /// <summary>
    /// Adds the type to the generated types.
    /// </summary>
    /// <param name="type"></param>
    public void Add(Type type)
    {
        Requires.NotNull(type, nameof(type));
            
        _allGeneratedTypes.Add(type);
        _currentTypeGeneratedTypes?.Add(type);
    }
    
    /// <summary>
    /// Gets the "generation stack" consisting of types generated for the current type (analogous to the call stack).
    /// </summary>
    /// <returns>The type generation stack.</returns>
    public IEnumerable<Type> GetTypeGenerationStack() => _currentTypeGeneratedTypes.Reverse();

    /// <summary>
    /// Checks if a type has already been generated.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool IsGenerated(Type type)
    {
        Requires.NotNull(type, nameof(type));
        return _allGeneratedTypes?.Contains(type) ?? false;
    }

    /// <summary>
    /// Checks if a type has already been generated for the currently generated type.
    /// This method also returns true if the argument is the currently generated type itself.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool IsGeneratedForType(Type type)
    {
        Requires.NotNull(type, nameof(type));
        return _currentTypeGeneratedTypes?.Contains(type) ?? false;
    }

    /// <summary>
    /// Begins type generation.
    /// </summary>
    public void BeginTypeGeneration() => _currentTypeGeneratedTypes = new List<Type>();

    /// <summary>
    /// Ends type generation.
    /// </summary>
    public void EndTypeGeneration() => _currentTypeGeneratedTypes = null;
}