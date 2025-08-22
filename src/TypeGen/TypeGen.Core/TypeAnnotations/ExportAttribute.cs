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
        private string _customHeader;
        private string _customBody;
        private string _customFooter;

        /// <summary>
        /// TypeScript file output directory
        /// </summary>
        public string OutputDir
        {
            get => _outputDir;
            set => _outputDir = FileSystemUtils.AsDirectory(value);
        }

        /// <summary>
        /// TypeScript file custom header
        /// </summary>
        public string CustomHeader
        {
            get => _customHeader;
            set => _customHeader = value;
        }

        /// <summary>
        /// TypeScript file custom body
        /// </summary>
        public string CustomBody
        {
            get => _customBody;
            set => _customBody = value;
        }
        
        /// <summary>
        /// TypeScript file custom Footer
        /// </summary>
        public string CustomFooter
        {
            get => _customFooter;
            set => _customFooter = value;
        }
        
        
    }
}
