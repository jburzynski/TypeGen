using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Cli
{
    internal static class StringExtensions
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
    }
}
