using System;
using System.Collections.Generic;
using System.Text;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Specifies custom base class declaration for a TypeScript class or interface.
    /// Base class declaration is empty if no content is specified.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TsCustomBaseAttribute : Attribute
    {
        /// <summary>
        /// Base class/interface type name
        /// </summary>
        public string Base { get; set; }

        /// <summary>
        /// The path of custom base type file to import (can be left null if no imports are required)
        /// </summary>
        public string ImportPath { get; set; }

        /// <summary>
        /// The original TypeScript base type name.
        /// This property should be used when the specified Base differs from the original base type name defined in the file under ImportPath.
        /// This property should only be used in conjunction with ImportPath.
        /// </summary>
        public string OriginalTypeName { get; set; }
        
        /// <summary>
        /// Whether default export is used for the referenced TypeScript type - used only in combination with ImportPath
        /// </summary>
        public bool IsDefaultExport { get; set; }

        public TsCustomBaseAttribute()
        {
        }

        /// <summary>
        /// TsCustomBaseAttribute constructor
        /// </summary>
        /// <param name="base">The base type name (or alias)</param>
        public TsCustomBaseAttribute(string @base)
        {
            Base = @base;
        }

        /// <summary>
        /// TsCustomBaseAttribute constructor
        /// </summary>
        /// <param name="base">The base type name (or alias)</param>
        /// <param name="importPath">The path of base type file to import (optional)</param>
        /// <param name="originalTypeName">The original TypeScript type name, defined in the file under ImportPath - used only if type alias is specified</param>
        /// <param name="isDefaultExport">Whether default export is used for the referenced TypeScript type - used only in combination with importPath</param>
        public TsCustomBaseAttribute(string @base, string importPath = null, string originalTypeName = null, bool isDefaultExport = false)
        {
            Base = @base;
            ImportPath = importPath;
            OriginalTypeName = originalTypeName;
            IsDefaultExport = isDefaultExport;
        }
    }
}
