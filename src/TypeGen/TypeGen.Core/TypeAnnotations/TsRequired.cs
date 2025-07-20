using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies a required TypeScript property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsRequiredAttribute : Attribute
    {
    }
}