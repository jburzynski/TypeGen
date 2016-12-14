using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Base class for 'ExportTs...' attributes
    /// </summary>
    public abstract class ExportAttribute : Attribute
    {
        /// <summary>
        /// TypeScript file output directory
        /// </summary>
        public string OutputDir { get; set; }
    }
}
