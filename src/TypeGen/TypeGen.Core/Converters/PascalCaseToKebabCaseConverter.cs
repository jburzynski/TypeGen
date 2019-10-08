using System;
using System.Reflection;
using System.Text.RegularExpressions;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Converters
{
    /// <summary>
    /// Converts PascalCase names to kebab-case names
    /// </summary>
    public class PascalCaseToKebabCaseConverter : IMemberNameConverter, ITypeNameConverter
    {
        /// <summary>
        /// Regex taken from http://stackoverflow.com/a/37301354
        /// </summary>
        private static readonly Regex _regex = new Regex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])");

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
            return _regex.Replace(name, "-$1").ToLowerInvariant();
        }
    }
}
