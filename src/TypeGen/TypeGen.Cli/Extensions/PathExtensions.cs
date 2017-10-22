using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TypeGen.Cli.Extensions
{
    internal static class PathExtensions
    {
        public static string ToAbsolutePath(this string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return path;
            return Path.IsPathRooted(path) ? path : Path.Combine(Directory.GetCurrentDirectory(), path);
        }
    }
}
