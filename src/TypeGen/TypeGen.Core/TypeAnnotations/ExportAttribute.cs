using System;

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
