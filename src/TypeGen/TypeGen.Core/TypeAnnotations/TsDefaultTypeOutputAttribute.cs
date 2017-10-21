using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Specifies the generated TypeScript type's default output directory.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsDefaultTypeOutputAttribute : Attribute
    {
        /// <summary>
        /// The file's default output directory
        /// </summary>
        public string OutputDir { get; set; }

        public TsDefaultTypeOutputAttribute(string outputDir)
        {
            OutputDir = outputDir;
        }
    }
}
