using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core.Services
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

        public TypeDependencyInfo(Type type, Attribute[] memberAttributes)
        {
            Type = type;
            MemberAttributes = memberAttributes;
        }

        /// <summary>
        /// The type dependency
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Custom attributes of the property or field that is of the dependent type
        /// </summary>
        public Attribute[] MemberAttributes { get; set; }
    }
}
