using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Core
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Checks if obj is of type T.
        /// Used for readable "is" negations ("is not").
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Is<T>(this object obj)
        {
            return obj is T;
        }
    }
}
