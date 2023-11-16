using System;
using System.Linq;
using System.Text.RegularExpressions;
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
                return char.ToUpperInvariant(value[0]) + value.Remove(0,1);

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
        /// Removes generic component from the TypeScript type name, e.g. "MyType&lt;T&gt;" becomes "MyType"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveTsTypeNameGenericComponent(this string value)
        {
            Requires.NotNull(value, nameof(value));
            return value.Split('<')[0];
        }

        /// <summary>
        /// Removes generic arguments from the type name.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveGenericArgumentsFromTypeName(this string value)
        {
            Requires.NotNull(value, nameof(value));
            return value.Split('[')[0];
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


        /// <summary>
        /// Ensures that a given string has a prefix. If it does not, it is added. Otherwise the string returned, unmodified.
        /// </summary>
        /// <param name="str">The initial string value</param>
        /// <param name="prefix">The prefix desired at the beginning of str</param>
        /// <param name="comparison">How the startswith comparison is done</param>
        /// <returns>prefix + str or str</returns>
        public static string EnsurePrefix(this string str, string prefix, StringComparison comparison = StringComparison.CurrentCulture)
        {
            Requires.NotNull(str, nameof(str));
            Requires.NotNull(prefix, nameof(prefix));

            if (!str.StartsWith(prefix, comparison))
                return prefix + str;
            return str;
        }

        /// <summary>
        /// Ensures that a given string has a postfix. If it does not, it is added. Otherwise the string returned, unmodified.
        /// </summary>
        /// <param name="str">The initial string value</param>
        /// <param name="postfix">The postfix desired at the end of str</param>
        /// <param name="comparison">How the endswith comparison is done</param>
        /// <returns>str + postfix or str</returns>
        public static string EnsurePostfix(this string str, string postfix, StringComparison comparison = StringComparison.CurrentCulture)
        {
            Requires.NotNull(str, nameof(str));
            Requires.NotNull(postfix, nameof(postfix));

            if (!str.EndsWith(postfix, comparison))
                return str + postfix;
            return str;
        }

        /// <summary>
        /// Ensures that a given string does NOT have a prefix as specified. If it does, it is removed. Otherwise the string returned, unmodified.
        /// </summary>
        /// <param name="str">The initial string value</param>
        /// <param name="prefix">The prefix that should be removed</param>
        /// <param name="comparison">How the startswith comparison is done</param>
        /// <returns>-prefix + str or str</returns>
        public static string EnsureRemovedPrefix(this string str, string prefix, StringComparison comparison = StringComparison.CurrentCulture)
        {
            Requires.NotNull(str, nameof(str));
            Requires.NotNull(prefix, nameof(prefix));

            if (str.StartsWith(prefix, comparison))
            {
                if (prefix.Length == str.Length)
                    return "";

                return str.Substring(prefix.Length);
            }
            return str;
        }

        /// <summary>
        /// Ensures that a given string does NOT have a postfix as specified. If it does, it is removed. Otherwise the string returned, unmodified.
        /// </summary>
        /// <param name="str">The initial string value</param>
        /// <param name="postfix">The postfix that should be removed</param>
        /// <param name="comparison">How the endswith comparison is done</param>
        /// <returns>str - postfix or str</returns>
        public static string EnsureRemovedPostfix(this string str, string postfix, StringComparison comparison = StringComparison.CurrentCulture)
        {
            Requires.NotNull(str, nameof(str));
            Requires.NotNull(postfix, nameof(postfix));

            if (str.EndsWith(postfix, comparison))
            {
                if (postfix.Length == str.Length)
                    return "";

                return str.Substring(0, str.Length - postfix.Length);
            }
            return str;
        }

        public static string NormalizeNewLines(this string str)
        {
            return Regex.Replace(str, @"\r\n|\n|\r", Environment.NewLine);
        }

        public static string SetIndentation(this string str, int indentationLength)
            => str.MakeIndentation(indentationLength, true);

        public static string AddIndentation(this string str, int indentationLength)
            => str.MakeIndentation(indentationLength, false);

        private static string MakeIndentation(this string str, int indentationLength, bool trimLinesStart)
        {
            var indentation = string.Join("", Enumerable.Repeat(" ", indentationLength));
            str = str.NormalizeNewLines();
            var lines = str.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return string.Join(Environment.NewLine, lines.Select(x => indentation + (trimLinesStart ? x.TrimStart() : x)));
        }
    }
}
