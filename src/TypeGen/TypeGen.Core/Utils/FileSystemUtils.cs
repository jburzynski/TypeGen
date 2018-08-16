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
    internal class FileSystemUtils
    {
        /// <summary>
        /// Split paths by seperator with \\ and /
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] SplitPathSeperator(string path)
        {
            Requires.NotNullOrEmpty(path, nameof(path));
            return path.Split('\\', '/');
        }

        public static string GetFileNameFromPath(string path)
        {
            Requires.NotNullOrEmpty(path, nameof(path));
            return SplitPathSeperator(path).Last();
        }

        public static string GetProjectFilePath(IFileSystem fileSystem, string projectFolder)
        {
            Requires.NotNull(fileSystem, nameof(fileSystem));
            Requires.NotNullOrEmpty(projectFolder, nameof(projectFolder));
            
            string fileName = fileSystem.GetDirectoryFiles(projectFolder)
                .Select(GetFileNameFromPath)
                .FirstOrDefault(n => n.EndsWith(".csproj"));

            return fileName == null ? null : Path.Combine(projectFolder, fileName);
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
            Requires.NotNull(pathFrom, nameof(pathFrom));
            Requires.NotNull(pathTo, nameof(pathTo));
            
            var pathFromUri = new Uri("file:///" + pathFrom);
            var pathToUri = new Uri("file:///" + pathTo);

            return pathFromUri.MakeRelativeUri(pathToUri).ToString();
        }
    }
}
