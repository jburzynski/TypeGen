using System;
using System.Linq;
using TypeGen.Core.Extensions;
using TypeGen.Core.Utils;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Base class for 'ExportTs...' attributes
    /// </summary>
    public abstract class ExportAttribute : Attribute
    {
        private string _outputDir;
        
        /// <summary>
        /// TypeScript file output directory
        /// </summary>
        public string OutputDir
        {
            get => _outputDir;
            set => _outputDir = FileSystemUtils.AsDirectory(value);
        }
    }
}
