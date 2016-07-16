using System;
using TypeGen.Core.Converters;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Indentifies a class that a TypeScript interface file should be generated for
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TsInterfaceAttribute : Attribute
    {
        /// <summary>
        /// TypeScript file output directory
        /// </summary>
        public string OutputDir { get; set; }
    }
}
