using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies a class that a TypeScript file should be generated for
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public class ExportTsStructAttribute : ExportAttribute
    {
    }
}
