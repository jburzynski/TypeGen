using System.Collections.Generic;

namespace TypeGen.Core.Storage
{
    internal interface IFileSystem
    {
        /// <summary>
        /// Writes a text file to the specified location
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        void SaveFile(string filePath, string content);

        /// <summary>
        /// Reads file as string
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string ReadFile(string filePath);

        /// <summary>
        /// Checks if the file exists
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        bool FileExists(string filePath);

        /// <summary>
        /// Searches recursively for a file in a directory
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        IEnumerable<string> GetFilesRecursive(string rootDirectory, string fileName);

        /// <summary>
        /// Checks if the directory exists
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        bool DirectoryExists(string directory);

        /// <summary>
        /// Gets all files in a directory (only files, not directories)
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        IEnumerable<string> GetDirectoryFiles(string directory);

        /// <summary>
        /// Gets path prefix required to navigate from pathFrom to pathTo.
        /// E.g. if path1=path/to/file.txt and path2=path/file.txt, this method will return "..\"
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <returns></returns>
        string GetPathDiff(string pathFrom, string pathTo);
    }
}