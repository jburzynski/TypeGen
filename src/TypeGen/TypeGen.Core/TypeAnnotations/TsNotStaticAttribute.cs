using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies a TypeScript property that is not static
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsNotStaticAttribute : Attribute
    {
    }
}