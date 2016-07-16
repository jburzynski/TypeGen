using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core
{
    public static class StringExtensions
    {
        /// <summary>
        /// Normalizes a path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string NormalizePath(this string path)
        {
            if (path == null) return null;
            if (path == "") return "";

            path = path.Replace('/', '\\');

            if (path.First() == '\\')
            {
                path = path.Remove(0, 1);
            }

            if (path != "" && path.Last() == '\\')
            {
                path = path.Remove(path.Length - 1);
            }

            return path;
        }

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
    }
}
