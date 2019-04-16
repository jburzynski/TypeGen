using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies a static TypeScript property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsStaticAttribute : Attribute
    {
    }
}