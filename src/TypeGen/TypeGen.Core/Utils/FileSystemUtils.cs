using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core.Utils
{
    /// <summary>
    /// File system-related utility class
    /// </summary>
    internal class FileSystemUtils
    {
        /// <summary>
        /// Split paths by seperator with \\ and /
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] SplitPathSeperator(string path)
        {
            return path.Split('\\', '/');
        }

        public static string GetFileNameFromPath(string path)
        {
            return SplitPathSeperator(path).Last();
        }
    }
}
