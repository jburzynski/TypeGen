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
        /// Checks if the file does not exist
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        bool FileDoesNotExist(string filePath);

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
        /// Gets all directories in a directory (only directories, not files)
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        IEnumerable<string> GetDirectoryDirectories(string directory);

        /// <summary>
        /// Gets the current directory
        /// </summary>
        /// <returns></returns>
        string GetCurrentDirectory();

        /// <summary>
        /// Clears the directory (removes all files and recursively removes all subdirectories)
        /// </summary>
        /// <param name="directory"></param>
        void ClearDirectory(string directory);

        /// <summary>
        /// Sets the current working directory
        /// </summary>
        /// <param name="directory"></param>
        void SetCurrentDirectory(string directory);
    }
}