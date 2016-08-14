using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TypeGen.Core.Extensions;

namespace TypeGen.Core
{
    /// <summary>
    /// Utility class
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Gets embedded resource as string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetEmbeddedResource(string name)
        {
            using (Stream stream = typeof (Utilities).Assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                {
                    throw new CoreException($"Could not find embedded resource '{name}'");
                }

                var contentBytes = new byte[stream.Length];
                stream.Read(contentBytes, 0, (int)stream.Length);
                return Encoding.ASCII.GetString(contentBytes);
            }
        }

        /// <summary>
        /// Gets a string value to use as a tab
        /// </summary>
        /// <param name="tabLength">The number of spaces per tab.</param>
        /// <returns></returns>
        public static string GetTabText(int tabLength)
        {
            var tabText = "";
            for (var i = 0; i < tabLength; i++)
            {
                tabText += " ";
            }
            return tabText;
        }

        /// <summary>
        /// Gets path prefix required to navigate from path1 to path2.
        /// E.g. if path1=path/to/file.txt and path2=path/file.txt, this method will return "..\".
        /// This method returns a path with a trailing slash if diff is not empty; otherwise returns an empty string.
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static string GetPathDiff(string path1, string path2)
        {
            if (string.IsNullOrEmpty(path1)) path1 = ".\\";
            if (string.IsNullOrEmpty(path2)) path2 = ".\\";

            path1 = Path.GetFullPath(path1).NormalizePath();
            path2 = Path.GetFullPath(path2).NormalizePath();

            string prefix = GetMaximalCommonPrefix(path1, path2);

            // remove common prefix from each path
            path1 = path1.ReplaceFirst(prefix, "").NormalizePath();
            path2 = path2.ReplaceFirst(prefix, "").NormalizePath();

            // calculate depth between path1 and path2
            int relativeDepth = path1 == "" ? 0 : path1.Split('\\').Length;

            var diff = "";
            relativeDepth.Times(i => { diff += "..\\"; });
            diff += path2;

            if (diff != "")
            {
                return diff.EndsWith("\\") ? diff : $"{diff}\\";
            }

            return "";
        }

        /// <summary>
        /// Gets maximal common prefix for two strings (case-insensitive)
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        private static string GetMaximalCommonPrefix(string str1, string str2)
        {
            int length = Math.Min(str1.Length, str2.Length);
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                if (char.ToUpperInvariant(str1[i]) != char.ToUpperInvariant(str2[i])) break;
                stringBuilder.Append(str1[i]);
            }

            return stringBuilder.ToString();
        }
    }
}
