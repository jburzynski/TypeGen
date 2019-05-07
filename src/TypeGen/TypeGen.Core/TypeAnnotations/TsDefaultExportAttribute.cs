using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Specifies custom base class declaration for a TypeScript class or interface.
    /// Base class declaration is empty if no content is specified.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum, Inherited = false)]
    public class TsDefaultExportAttribute : Attribute
    {
        public bool Enabled { get; set; }
        
        public TsDefaultExportAttribute(bool enabled = true)
        {
            Enabled = enabled;
        }
    }
}