using System;
using System.Linq;

namespace TypeGen.Core.Converters
{
    /// <summary>
    /// Converts PascalCase names to camelCase names
    /// </summary>
    public class PascalCaseToCamelCaseConverter : INameConverter, ITypeNameConverter
    {
        public string Convert(string name)
        {
            return ConvertTypeInvariant(name);
        }

        public string Convert(string name, Type type)
        {
            return ConvertTypeInvariant(name);
        }

        private static string ConvertTypeInvariant(string name)
        {
            char firstChar = char.ToLowerInvariant(name[0]);
            return firstChar + name.Remove(0, 1);
        }
    }
}
