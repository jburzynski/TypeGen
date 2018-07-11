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
        public IEnumerable<string> GetFilesRecursive(string rootDirectory, string fileName)
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
        /// Gets all files in a directory (only files, not directories)
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public IEnumerable<string> GetDirectoryFiles(string directory) => Directory.GetFiles(directory);
    }
}
