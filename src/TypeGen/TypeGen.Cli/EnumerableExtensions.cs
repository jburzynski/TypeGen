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
        /// Maps an array to a new array.
        /// Returns the new (mapped) array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="mapFunc"></param>
        /// <returns></returns>
        public static T[] Map<T>(this T[] array, Func<T, T> mapFunc)
        {
            var result = new T[array.Length];
            for (var i = 0; i < array.Length; i++)
            {
                result[i] = mapFunc(array[i]);
            }
            return result;
        }
    }
}
