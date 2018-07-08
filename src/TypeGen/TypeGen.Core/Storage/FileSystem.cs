using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TypeGen.Core.Storage
{
    /// <summary>
    /// Represents the file system
    /// </summary>
    internal class FileSystem : IFileSystem
    {
        /// <inheritdoc />
        public void SaveFile(string filePath, string content)
        {
            new FileInfo(filePath).Directory?.Create();
            File.WriteAllText(filePath, content);
        }

        /// <inheritdoc />
        public string ReadFile(string filePath) => File.ReadAllText(filePath);

        /// <inheritdoc />
        public bool FileExists(string filePath) => File.Exists(filePath);

        /// <inheritdoc />
        public IEnumerable<string> GetFilesRecursive(string rootDirectory, string fileName)
        {
            return Directory.GetFiles(rootDirectory, fileName, SearchOption.AllDirectories);
        }

        /// <inheritdoc />
        public bool DirectoryExists(string directory) => Directory.Exists(directory);

        /// <inheritdoc />
        public IEnumerable<string> GetDirectoryFiles(string directory) => Directory.GetFiles(directory);

        /// <inheritdoc />
        public string GetPathDiff(string pathFrom, string pathTo)
        {
            var pathFromUri = new Uri("file:///" + pathFrom);
            var pathToUri = new Uri("file:///" + pathTo);

            return pathFromUri.MakeRelativeUri(pathToUri).ToString();
        }
    }
}
