using System.Collections.Generic;
using System.Linq;
using TypeGen.Core.Converters;

namespace TypeGen.Cli.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Checks if an array has the specified index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool HasIndex<T>(this T[] array, int index)
        {
            return array?.Length >= index + 1;
        }

        /// <summary>
        /// Maps an enumerable to an enumerable of the elements' type names
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetTypeNames(this IEnumerable<IConverter> enumerable)
        {
            return enumerable
                .Select(c => c.GetType().Name);
        }

        /// <summary>
        /// Checks if an enumerable is empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return true;
            return !enumerable.Any();
        }
    }
}
