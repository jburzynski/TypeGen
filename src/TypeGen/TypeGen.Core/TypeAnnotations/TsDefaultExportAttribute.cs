using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Specifies that default export will be used for a TypeScript type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Enum, Inherited = false)]
    public class TsDefaultExportAttribute : Attribute
    {
        public bool Enabled { get; set; }
        
        public TsDefaultExportAttribute(bool enabled = true)
        {
            Enabled = enabled;
        }
    }
}