using System;

namespace TypeGen.Core
{
    /// <summary>
    /// Interface for type name converters
    /// </summary>
    public interface ITypeNameConverter
    {
        /// <summary>
        /// Converts a type's name to a new name.
        /// Conversion can optionally depend on the type, which name is being changed.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type">The type, which name is being changed</param>
        /// <returns>Converted name</returns>
        string Convert(string name, Type type);
    }
}