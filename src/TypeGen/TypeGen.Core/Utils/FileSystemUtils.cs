using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TypeGen.Core.Storage;

namespace TypeGen.Core.Utils
{
    /// <summary>
    /// File system-related utility class
    /// </summary>
    internal class FileSystemUtils
    {
        /// <summary>
        /// Split paths by seperator with \\ and /
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] SplitPathSeperator(string path)
        {
            return path.Split('\\', '/');
        }

        public static string GetFileNameFromPath(string path)
        {
            return SplitPathSeperator(path).Last();
        }

        public static string GetProjectFilePath(FileSystem fileSystem, string projectFolder)
        {
            string fileName = fileSystem.GetDirectoryFiles(projectFolder)
                .Select(GetFileNameFromPath)
                .FirstOrDefault(n => n.EndsWith(".csproj"));

            return fileName == null ? null : Path.Combine(projectFolder, fileName);
        }
    }
}
