using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Specifies the generated TypeScript type's default output directory.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsDefaultTypeOutputAttribute : Attribute
    {
        public string OutputDir { get; private set; }

        public TsDefaultTypeOutputAttribute(string outputDir)
        {
            OutputDir = outputDir;
        }
    }
}
