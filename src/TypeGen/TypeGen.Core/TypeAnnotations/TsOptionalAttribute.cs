using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies an optional interface property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsOptionalAttribute : Attribute
    {
    }
}
