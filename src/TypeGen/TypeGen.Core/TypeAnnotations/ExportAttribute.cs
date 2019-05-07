using System;
using System.Linq;
using TypeGen.Core.Extensions;

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
            get
            {
                if (string.IsNullOrWhiteSpace(_outputDir)) return _outputDir;
                return _outputDir.Last().In('\\', '/') ? _outputDir : _outputDir + '/';
            }
            set => _outputDir = value;
        }
    }
}
