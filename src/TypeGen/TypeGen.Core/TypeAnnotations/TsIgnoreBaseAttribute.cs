using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Marked TypeScript classes/interfaces will not have base type declaration.
    /// Also, base classes/interfaces will not be generated if they're not marked with an ExportTs... attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct, Inherited = false)]
    public class TsIgnoreBaseAttribute : Attribute
    {
    }
}
