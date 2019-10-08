using System;
using System.Linq;
using System.Reflection;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Converters
{
    /// <summary>
    /// Converts PascalCase names to camelCase names
    /// </summary>
    public class PascalCaseToCamelCaseConverter : IMemberNameConverter, ITypeNameConverter
    {
        public string Convert(string name, MemberInfo memberInfo)
        {
            Requires.NotNullOrEmpty(name, nameof(name));
            return ConvertTypeInvariant(name);
        }

        public string Convert(string name, Type type)
        {
            Requires.NotNullOrEmpty(name, nameof(name));
            return ConvertTypeInvariant(name);
        }

        private static string ConvertTypeInvariant(string name)
        {
            char firstChar = char.ToLowerInvariant(name[0]);
            return firstChar + name.Remove(0, 1);
        }
    }
}
