using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core.Services
{
    /// <summary>
    /// A comparer class that compares types of TypeDependencyInfo instances
    /// </summary>
    public class TypeDependencyInfoTypeComparer<T> : IEqualityComparer<T> where T: TypeDependencyInfo
    {
        public bool Equals(T x, T y)
        {
            return x.Type == y.Type;
        }

        public int GetHashCode(T obj)
        {
            return obj.Type.GetHashCode();
        }
    }
}
