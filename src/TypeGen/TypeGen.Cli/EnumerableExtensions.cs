using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Cli
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
    }
}
