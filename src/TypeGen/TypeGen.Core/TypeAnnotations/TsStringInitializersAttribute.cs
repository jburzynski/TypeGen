using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Specifies whether TypeScript string initializers should be used for an enum
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class TsStringInitializersAttribute : Attribute
    {
        public bool Enabled { get; set; }
        
        public TsStringInitializersAttribute(bool enabled = true)
        {
            Enabled = enabled;
        }
    }
}