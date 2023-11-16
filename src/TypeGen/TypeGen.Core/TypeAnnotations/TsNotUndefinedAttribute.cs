using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies a TypeScript property that cannot be set to undefined (used only with enabled strict null checking mode)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsNotUndefinedAttribute : Attribute
    {
    }
}
