using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Indentifies an enum that a TypeScript file should be generated for
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class ExportTsEnumAttribute : ExportAttribute
    {
        /// <summary>
        /// Specifies whether an enum should be exported as TypeScript const enum
        /// </summary>
        public bool IsConst { get; set; }
    }
}
