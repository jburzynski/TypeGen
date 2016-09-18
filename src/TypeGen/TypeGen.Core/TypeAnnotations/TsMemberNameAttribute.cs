using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Specifies the generated member's name
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsMemberNameAttribute : Attribute
    {
        /// <summary>
        /// The member's name
        /// </summary>
        public string Name { get; set; }

        public TsMemberNameAttribute(string name)
        {
            Name = name;
        }
    }
}
