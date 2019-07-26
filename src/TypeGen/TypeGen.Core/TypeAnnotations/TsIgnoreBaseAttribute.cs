using System;
using System.Collections.Generic;
using System.Text;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Marked TypeScript classes/interfaces will not have base type declaration.
    /// Also, base classes/interfaces will not be generated if they're not marked with an ExportTs... attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class TsIgnoreBaseAttribute : Attribute
    {
    }
}
