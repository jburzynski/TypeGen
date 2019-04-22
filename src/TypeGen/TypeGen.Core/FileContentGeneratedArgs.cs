using System;

namespace TypeGen.Core
{
    /// <summary>
    /// Event args for the Generator.FileContentGenerated event
    /// </summary>
    public class FileContentGeneratedArgs : EventArgs
    {
        /// <summary>
        /// The type for which the file was generated
        /// </summary>
        public Type Type { get; set; }
        
        /// <summary>
        /// The generated file's path
        /// </summary>
        public string FilePath { get; set; }
        
        /// <summary>
        /// The generated file content
        /// </summary>
        public string FileContent { get; set; }

        public FileContentGeneratedArgs(Type type, string filePath, string fileContent)
        {
            Type = type;
            FilePath = filePath;
            FileContent = fileContent;
        }
    }
}