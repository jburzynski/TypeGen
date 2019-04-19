using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies a readonly TypeScript property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsReadonlyAttribute : Attribute
    {
    }
}