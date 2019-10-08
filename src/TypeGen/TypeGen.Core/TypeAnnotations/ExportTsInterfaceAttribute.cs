using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies a class that a TypeScript interface file should be generated for
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class ExportTsInterfaceAttribute : ExportAttribute
    {
    }
}
