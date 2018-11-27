using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TypeGen.Core.Storage;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Utils
{
    /// <summary>
    /// File system-related utility class
    /// </summary>
    internal static class FileSystemUtils
    {
        /// <summary>
        /// Split paths by separator with \\ and /
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] SplitPathSeparator(string path)
        {
            Requires.NotNullOrEmpty(path, nameof(path));
            return path.Split('\\', '/');
        }

        public static string GetFileNameFromPath(string path)
        {
            Requires.NotNullOrEmpty(path, nameof(path));
            return SplitPathSeparator(path).Last();
        }

        public static string GetProjectFilePath(string projectFolder, IFileSystem fileSystem)
        {
            Requires.NotNull(fileSystem, nameof(fileSystem));
            Requires.NotNullOrEmpty(projectFolder, nameof(projectFolder));
            projectFolder = projectFolder.Replace('\\', '/');
            string fileName = fileSystem.GetDirectoryFiles(projectFolder)
                .Select(GetFileNameFromPath)
                .FirstOrDefault(n => n.EndsWith(".csproj"));

            return fileName == null ? null : Path.Combine(projectFolder, fileName).Replace('\\', '/');
        }

        /// <summary>
        /// Gets path prefix required to navigate from pathFrom to pathTo.
        /// E.g. if path1=path/to/file.txt and path2=path/file.txt, this method will return "../file.txt"
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <returns></returns>
        public static string GetPathDiff(string pathFrom, string pathTo)
        {
            var pathFromUri = new Uri("file:///" + pathFrom?.Replace('\\', '/'));
            var pathToUri = new Uri("file:///" + pathTo?.Replace('\\', '/'));

            return pathFromUri.MakeRelativeUri(pathToUri).ToString();
        }
    }
}
