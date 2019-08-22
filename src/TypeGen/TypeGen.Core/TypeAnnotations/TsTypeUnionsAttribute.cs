using System;
using System.Collections.Generic;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Specifies TypeScript type unions (excluding the main type) for a property or field
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsTypeUnionsAttribute : Attribute
    {
        public IEnumerable<string> TypeUnions { get; set; }

        public TsTypeUnionsAttribute(IEnumerable<string> typeUnions)
        {
            TypeUnions = typeUnions;
        }

        public TsTypeUnionsAttribute(params string[] typeUnions)
            : this((IEnumerable<string>)typeUnions)
        {
        }
    }
}