using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core.Extensions
{
    internal static class FileSystemExtensions
    {
        public static string NormalizePath(this string path)
        {
            if (path == null) return null;
            if (path == "") return "";

            if (path != "" && (path.Last() == '\\' || path.Last() == '/'))
            {
                path = path.Remove(path.Length - 1);
            }

            return path;
        }
    }
}
