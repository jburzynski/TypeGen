using System;
using System.Collections.Generic;
using System.Text;

namespace TypeGen.Types
{
    /// <summary>
    /// Specifies a default value for a TypeScript property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
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
