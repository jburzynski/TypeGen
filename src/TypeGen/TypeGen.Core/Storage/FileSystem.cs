using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TypeGen.Core.Extensions;

namespace TypeGen.Core.Storage
{
    /// <summary>
    /// Contains logic for manipulating the file system
    /// </summary>
    internal class FileSystem
    {
        /// <summary>
        /// Writes a text file to the specified location
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public void SaveFile(string filePath, string content)
        {
            new FileInfo(filePath).Directory?.Create();
            File.WriteAllText(filePath, content);
        }

        /// <summary>
        /// Reads file as string
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string ReadFile(string filePath) => File.ReadAllText(filePath);

        /// <summary>
        /// Checks if the file exists
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool FileExists(string filePath) => File.Exists(filePath);

        /// <summary>
        /// Gets path prefix required to navigate from path1 to path2.
        /// E.g. if path1=path/to/file.txt and path2=path/file.txt, this method will return "..\".
        /// This method returns a path with a trailing slash if diff is not empty; otherwise returns an empty string.
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public string GetPathDiff(string path1, string path2)
        {
            if (string.IsNullOrEmpty(path1)) path1 = ".\\";
            if (string.IsNullOrEmpty(path2)) path2 = ".\\";

            path1 = Path.GetFullPath(path1).NormalizePath();
            path2 = Path.GetFullPath(path2).NormalizePath();

            string prefix = GetMaximalCommonPathPrefix(path1, path2);

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
        /// Gets maximal common path prefix for two absolute, normalized paths (case-insensitive).
        /// The resulting prefix doesn't end with a slash.
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        private string GetMaximalCommonPathPrefix(string path1, string path2)
        {
            string[] path1Parts = path1.Split('\\');
            string[] path2Parts = path2.Split('\\');

            int length = Math.Min(path1Parts.Length, path2Parts.Length);
            var result = "";

            for (var i = 0; i < length; i++)
            {
                if (path1Parts[i] != path2Parts[i]) break;
                result += $"{path1Parts[i]}\\";
            }

            return result.EndsWith("\\") ? result.Remove(result.Length - 1) : result;
        }
    }
}
