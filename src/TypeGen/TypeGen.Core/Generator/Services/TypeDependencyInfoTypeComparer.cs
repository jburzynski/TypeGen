using System.Collections.Generic;

namespace TypeGen.Core.Generator.Services
{
    /// <summary>
    /// A comparer class that compares types of TypeDependencyInfo instances
    /// </summary>
    internal class TypeDependencyInfoTypeComparer<T> : IEqualityComparer<T> where T: TypeDependencyInfo
    {
        public bool Equals(T x, T y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            return x.Type == y.Type;
        }

        public int GetHashCode(T obj)
        {
            return obj.Type?.GetHashCode() ?? 0;
        }
    }
}
