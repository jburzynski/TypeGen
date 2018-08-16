using System;
using System.Globalization;
using System.Linq;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Replaces the first occurrence of "search" in "text" with "replace"
        /// </summary>
        /// <param name="text"></param>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            Requires.NotNull(text, nameof(text));
            Requires.NotNull(search, nameof(search));

            int pos = text.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        /// <summary>
        /// Converts a string to TitleCase format
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string value)
        {
            Requires.NotNull(value, nameof(value));
            
            string[] tokens = value.ToLowerInvariant().Split(new[] { " ", "_" }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                tokens[i] = token.Substring(0, 1).ToUpperInvariant() + token.Substring(1);
            }

            return string.Join("", tokens);
        }

        /// <summary>
        /// Removes arity information from type name
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveTypeArity(this string value)
        {
            Requires.NotNull(value, nameof(value));
            return value.Split('`')[0];
        }
    }
}
