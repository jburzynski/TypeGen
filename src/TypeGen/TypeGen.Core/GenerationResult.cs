using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core
{
    /// <summary>
    /// Represents a result from file generation
    /// </summary>
    public struct GenerationResult
    {
        /// <summary>
        /// Base output directory of the generator at the time of generation
        /// </summary>
        public string BaseOutputDirectory { get; set; }

        /// <summary>
        /// Collection of generated files' paths (relative to BaseOutputDirectory)
        /// </summary>
        public IEnumerable<string> GeneratedFiles { get; set; }
    }
}
