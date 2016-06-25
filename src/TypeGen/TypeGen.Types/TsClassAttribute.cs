using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Types
{
    /// <summary>
    /// Indentifies a class that a TypeScript file should be generated for
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TsClassAttribute : Attribute
    {
        /// <summary>
        /// TypeScript file output directory
        /// </summary>
        public string OutputDir { get; set; }
    }
}
