using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core
{
    /// <summary>
    /// Options for generating TypeScript files
    /// </summary>
    public class GeneratorOptions
    {
        /// <summary>
        /// Whether to use explicit "public" accessor. Default is "false".
        /// </summary>
        public bool ExplicitPublicAccessor { get; set; }
    }
}
