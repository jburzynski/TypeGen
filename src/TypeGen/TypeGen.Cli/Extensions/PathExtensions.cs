using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TypeGen.Core.Storage;
using TypeGen.Core.Validation;

namespace TypeGen.Cli.Extensions
{
    internal static class PathExtensions
    {
        public static string ToAbsolutePath(this string path, IFileSystem fileSystem)
        {
            if (string.IsNullOrWhiteSpace(path)) return path;
            return path.RelativeOrRooted(fileSystem.GetCurrentDirectory());
        }

        public static string RelativeOrRooted(this string path, string basePath)
        {
            Requires.NotNull(path, nameof(path));
            Requires.NotNull(basePath, nameof(basePath));
            
            return Path.IsPathRooted(path) ? path : Path.Combine(basePath, path);
        }
    }
}
