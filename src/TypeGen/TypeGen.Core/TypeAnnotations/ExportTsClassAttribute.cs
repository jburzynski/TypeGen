using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Indentifies a class that a TypeScript file should be generated for
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ExportTsClassAttribute : ExportAttribute
    {
    }
}
