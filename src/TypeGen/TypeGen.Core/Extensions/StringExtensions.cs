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
            if (tokens.Length == 1 && value != value.ToUpperInvariant())
                return char.ToUpperInvariant(value[0]) + value.Substring(1);

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
        
        /// <summary>
        /// Removes generic component from the type, e.g. "MyType<T>" becomes "MyType"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveTypeGenericComponent(this string value)
        {
            Requires.NotNull(value, nameof(value));
            return value.Split('<')[0];
        }

        /// <summary>
        /// Gets the type from a TypeScript type union indicated by the given index.
        /// E.g. the following string: "Date | null | undefined" is a TS type union with 3 types and each of these types can be accessed with an index from 0 to 2.
        /// </summary>
        /// <param name="value">The TypeScript type union string</param>
        /// <param name="index">The index of the type in the type union to retrieve</param>
        /// <returns>The type from the given TS type union indicated by the index</returns>
        public static string GetTsTypeUnion(this string value, int index)
        {
            Requires.NotNull(value, nameof(value));
            return value.Split('|')[index].Trim();
        }
    }
}
