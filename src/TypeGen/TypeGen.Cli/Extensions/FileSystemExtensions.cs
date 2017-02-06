using System.Linq;

namespace TypeGen.Cli.Extensions
{
    /// <summary>
    /// Extensions for filesystem-related operations
    /// </summary>
    internal static class FileSystemExtensions
    {
        /// <summary>
        /// Normalizes a path to [..\ | .\]this\path\format
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string NormalizePath(this string path)
        {
            if (path == null) return null;
            if (path == "") return "";

            path = path.Replace('/', '\\');

            if (path.First() == '\\')
            {
                path = path.Remove(0, 1);
            }

            if (path != "" && path.Last() == '\\')
            {
                path = path.Remove(path.Length - 1);
            }

            return path;
        }

        /// <summary>
        /// Adds a new path segment to the existing path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newSection"></param>
        /// <returns></returns>
        public static string ConcatPath(this string path, string newSection)
        {
            if (string.IsNullOrEmpty(path) && string.IsNullOrEmpty(newSection)) return null;

            if (string.IsNullOrEmpty(path)) return newSection;
            if (string.IsNullOrEmpty(newSection)) return path;

            return path.NormalizePath() + '\\' + newSection.NormalizePath();
        }
    }
}
