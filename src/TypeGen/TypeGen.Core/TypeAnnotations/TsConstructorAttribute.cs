using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies a property that should be required as a constructor parameter when generating a TypeScript file
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsConstructorAttribute : Attribute
    {
    }
}
