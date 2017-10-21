using System;
using System.Collections.Generic;
using System.Text;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Specifies custom base class definition for a TypeScript class or interface.
    /// Base class definition is empty if no content is specified.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TsCustomBaseAttribute : Attribute
    {
        private string _base;

        /// <summary>
        /// Base class/interface definition text
        /// </summary>
        public string Base {
            get => _base;
            set => _base = string.IsNullOrWhiteSpace(value) ? "" : $" extends {value}";
        }

        public TsCustomBaseAttribute()
        {
        }

        public TsCustomBaseAttribute(string @base)
        {
            Base = @base;
        }
    }
}
