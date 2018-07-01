using System;
using System.Collections.Generic;
using System.Reflection;

namespace TypeGen.Core
{
    /// <summary>
    /// Interface for a TypeScript generator class
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        /// Generates TypeScript files for C# files in assemblies
        /// </summary>
        /// <param name="assemblies"></param>
        GenerationResult Generate(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Generates TypeScript files for C# files in an assembly
        /// </summary>
        /// <param name="assembly"></param>
        GenerationResult Generate(Assembly assembly);

        /// <summary>
        /// Generate TypeScript files for a given type
        /// </summary>
        /// <param name="type"></param>
        GenerationResult Generate(Type type);
    }
}