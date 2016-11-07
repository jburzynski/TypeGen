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
        public bool DirectoryExists(string directory) => Directory.Exists(directory);

        public string[] GetDirectoryFiles(string directory) => Directory.GetFiles(directory);

        public bool FileExists(string filePath) => File.Exists(filePath);

        public string ReadFile(string filePath) => File.ReadAllText(filePath);
    }
}
