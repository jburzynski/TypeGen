using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies a property that should be ignored when generating a TypeScript file
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TsIgnoreAttribute : Attribute
    {
    }
}
