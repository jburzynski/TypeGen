using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies a TypeScript property that is not readonly
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsNotReadonlyAttribute : Attribute
    {
    }
}