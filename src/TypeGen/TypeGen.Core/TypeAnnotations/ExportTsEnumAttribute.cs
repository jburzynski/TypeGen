using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Indentifies an enum that a TypeScript file should be generated for
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class ExportTsEnumAttribute : ExportAttribute
    {
        public bool IsConst { get; set; }
    }
}
