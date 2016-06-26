using System;
using System.Collections.Generic;
using System.Text;

namespace TypeGen.Types
{
    /// <summary>
    /// Identifies a property that should be ignored when generating a TypeScript file
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TsIgnoreAttribute : Attribute
    {
    }
}
