using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Specifies a default value for a TypeScript property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsDefaultValueAttribute : Attribute
    {
        /// <summary>
        /// Default value for a property
        /// </summary>
        public string DefaultValue { get; set; }

        public TsDefaultValueAttribute(string defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}
