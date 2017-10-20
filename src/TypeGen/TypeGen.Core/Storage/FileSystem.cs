using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TypeGen.Core.Extensions;
using TypeGen.Core.Utils;

namespace TypeGen.Core.Storage
{
    /// <summary>
    /// Represents the file system
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
        /// Searches recursively for a file in a directory
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string[] GetFilesRecursive(string rootDirectory, string fileName)
        {
            return Directory.GetFiles(rootDirectory, fileName, SearchOption.AllDirectories);
        }

        /// <summary>
        /// Checks if the directory exists
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public bool DirectoryExists(string directory) => Directory.Exists(directory);

        /// <summary>
        /// Gets all files in a directory
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public string[] GetDirectoryFiles(string directory) => Directory.GetFiles(directory);

        /// <summary>
        /// Gets path prefix required to navigate from pathFrom to pathTo.
        /// E.g. if path1=path/to/file.txt and path2=path/file.txt, this method will return "..\"
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <returns></returns>
        public string GetPathDiff(string pathFrom, string pathTo)
        {
            var pathFromUri = new Uri("file:///" + pathFrom);
            var pathToUri = new Uri("file:///" + pathTo);

            return pathFromUri.MakeRelativeUri(pathToUri).ToString();
        }
    }
}
