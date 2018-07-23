using System;

namespace TypeGen.Core.Business
{
    /// <summary>
    /// Contains information about a type dependency.
    /// Type dependency is a type which the exported type depends on (complex type of a property or field).
    /// </summary>
    internal class TypeDependencyInfo
    {
        public TypeDependencyInfo()
        {
        }

        public TypeDependencyInfo(Type type, Attribute[] memberAttributes = null, bool isBase = false)
        {
            Type = type;
            MemberAttributes = memberAttributes;
            IsBase = isBase;
        }

        /// <summary>
        /// The type dependency
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Custom attributes of the property or field that is of the dependent type
        /// </summary>
        public Attribute[] MemberAttributes { get; set; }

        /// <summary>
        /// Indicates whether type dependency is a base class type
        /// </summary>
        public bool IsBase { get; set; }
    }
}
