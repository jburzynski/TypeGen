using System;
using System.Collections.Generic;
using System.Text;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies a TypeScript property that can be set to undefined (used only with enabled strict null checking mode)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsUndefinedAttribute : Attribute
    {
    }
}
