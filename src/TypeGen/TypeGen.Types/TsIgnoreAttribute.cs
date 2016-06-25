using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
