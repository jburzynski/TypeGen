using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
