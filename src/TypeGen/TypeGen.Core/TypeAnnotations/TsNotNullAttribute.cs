using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies a non-nullable TypeScript property (used only with enabled strict null checking mode)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsNotNullAttribute : Attribute
    {
    }
}
