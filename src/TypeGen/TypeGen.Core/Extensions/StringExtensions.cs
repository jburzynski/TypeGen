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
        /// Converts a string to TitleCase format.
        /// Uses the current thread's culture info for conversion.
        /// Source: http://theburningmonk.com/2010/08/dotnet-tips-string-totitlecase-extension-methods/
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string value)
        {
            CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(value.ToLower());
        }

        /// <summary>
        /// Converts a string to TitleCase format.
        /// Uses the culture info with the specified name.
        /// Source: http://theburningmonk.com/2010/08/dotnet-tips-string-totitlecase-extension-methods/
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfoName"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string value, string cultureInfoName)
        {
            var cultureInfo = new CultureInfo(cultureInfoName);
            return cultureInfo.TextInfo.ToTitleCase(value.ToLower());
        }

        /// <summary>
        /// Converts a string to TitleCase format.
        /// Uses the specified culture info.
        /// Source: http://theburningmonk.com/2010/08/dotnet-tips-string-totitlecase-extension-methods/
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string value, CultureInfo cultureInfo)
        {
            return cultureInfo.TextInfo.ToTitleCase(value.ToLower());
        }

        /// <summary>
        /// Determines whether the string is null or consists only of whitespace characters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrWhitespace(this string value)
        {
            return value == null || value.All(char.IsWhiteSpace);
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
