using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Cli.Utils
{
    /// <summary>
    /// File system-related utility class
    /// </summary>
    internal class FileSystemUtils
    {
        public static string GetFileNameFromPath(string path)
        {
            return path.Split('\\').Last();
        }
    }
}
