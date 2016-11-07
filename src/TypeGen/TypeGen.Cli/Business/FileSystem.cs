using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Cli.Business
{
    internal class FileSystem
    {
        public bool DirectoryExists(string directory)
        {
            return Directory.Exists(directory);
        }

        public string[] GetDirectoryFiles(string directory)
        {
            return Directory.GetFiles(directory);
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
