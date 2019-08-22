using System;

namespace TypeGen.Core
{
    /// <summary>
    /// Represents flags used in TypeScript strict null checking mode
    /// </summary>
    [Flags]
    public enum StrictNullTypeUnionFlags
    {
        /// <summary>
        /// Not null and not undefined
        /// </summary>
        None = 0,

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