using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TypeGen.Core.Storage;

namespace TypeGen.Cli.Extensions
{
    internal static class PathExtensions
    {
        public static string ToAbsolutePath(this string path, IFileSystem fileSystem)
        {
            if (string.IsNullOrWhiteSpace(path)) return path;
            return Path.IsPathRooted(path) ? path : Path.Combine(fileSystem.GetCurrentDirectory(), path);
        }
    }
}
