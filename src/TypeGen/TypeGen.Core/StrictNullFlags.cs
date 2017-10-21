using System;

namespace TypeGen.Core
{
    /// <summary>
    /// Represents flags used in TypeScript strict null checking mode
    /// </summary>
    [Flags]
    public enum StrictNullFlags
    {
        /// <summary>
        /// Not null and not undefined
        /// </summary>
        Regular = 0,

        /// <summary>
        /// Null
        /// </summary>
        Null = 1,

        /// <summary>
        /// Undefined
        /// </summary>
        Undefined = 2
    }
}