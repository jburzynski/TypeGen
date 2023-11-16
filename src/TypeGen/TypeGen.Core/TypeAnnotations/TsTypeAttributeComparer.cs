using System.Collections.Generic;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// A comparer class that compares TsTypeAttribute properties
    /// </summary>
    internal class TsTypeAttributeComparer : IEqualityComparer<TsTypeAttribute>
    {
        public bool Equals(TsTypeAttribute x, TsTypeAttribute y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            return x.TypeName == y.TypeName &&
                   x.ImportPath == y.ImportPath &&
                   x.OriginalTypeName == y.OriginalTypeName;
        }

        public int GetHashCode(TsTypeAttribute obj)
        {
            const int prime = 4534549;
            unchecked
            {
                return prime * (obj.TypeName?.GetHashCode() ?? 0 +
                                obj.ImportPath?.GetHashCode() ?? 0 +
                                obj.OriginalTypeName?.GetHashCode() ?? 0);
            }
        }
    }
}
