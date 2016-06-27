using System;
using System.Text.RegularExpressions;

namespace TypeGen.Core.Converters
{
    /// <summary>
    /// Converts PascalCase names to kebab-case names
    /// </summary>
    public class PascalCaseToKebabCaseConverter : INameConverter, ITypeNameConverter
    {
        /// <summary>
        /// Regex taken from http://stackoverflow.com/a/37301354
        /// All credit to Mikhail.
        /// </summary>
        private static readonly Regex _regex
            = new Regex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", RegexOptions.Compiled);

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
            return _regex.Replace(name, "-$1").ToLowerInvariant();
        }
    }
}
