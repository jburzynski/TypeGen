using System;
using System.Globalization;
using System.Linq;

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
            if (text == null) return null;

            int pos = text.IndexOf(search);
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
            string[] tokens = value.ToLowerInvariant().Split(new[] { " ", "_" }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                tokens[i] = token.Substring(0, 1).ToUpperInvariant() + token.Substring(1);
            }

            return string.Join("_", tokens);
        }

        /// <summary>
        /// Determines whether the string is null or consists only of whitespace characters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrWhitespace(this string value)
        {
            return value == null || value.ToCharArray().All(char.IsWhiteSpace);
        }

        /// <summary>
        /// Removes arity information from type name
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveTypeArity(this string value)
        {
            return value.Split('`')[0];
        }
    }
}
