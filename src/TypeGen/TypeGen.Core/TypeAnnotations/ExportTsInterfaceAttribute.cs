using System;
using TypeGen.Core.Converters;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Indentifies a class that a TypeScript interface file should be generated for
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ExportTsInterfaceAttribute : ExportAttribute
    {
    }
}
