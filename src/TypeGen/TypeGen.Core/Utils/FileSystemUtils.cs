using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core.Utils
{
    /// <summary>
    /// File system-related utility class
    /// </summary>
    public class FileSystemUtils
    {
        public static string GetFileNameFromPath(string path)
        {
            return path.Split('\\').Last();
        }
    }
}
