using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TypeGen.Core.Extensions;
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

        /// <summary>
        /// Gets path prefix required to navigate from pathFrom to pathTo.
        /// E.g. if path1=path/to/file.txt and path2=path/file.txt, this method will return "../file.txt"
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <returns></returns>
        public static string GetPathDiff(string pathFrom, string pathTo)
        {
            var pathFromUri = new Uri("file:///root/" + pathFrom?.Replace('\\', '/').EnsurePostfix("/"));
            var pathToUri = new Uri("file:///root/" + pathTo?.Replace('\\', '/').EnsurePostfix("/"));

            return pathFromUri.MakeRelativeUri(pathToUri).ToString();
        }

        public static string AsDirectory(string path)
        {
            if (path == null) return null;
            if (path == "") return "./";
            
            return path.Last().In('\\', '/') ? path : path + '/';
        }
    }
}
