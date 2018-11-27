using System;
using System.Collections.Generic;
using TypeGen.Core.Business;
using TypeGen.Core.Extensions;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Utils
{
    /// <summary>
    /// Determines empty values for types
    /// </summary>
    internal class EmptyValue
    {
        public static bool ExistsFor(string tsTypeName)
        {
            Requires.NotNull(tsTypeName, nameof(tsTypeName));
            
            return tsTypeName.In("Object", "boolean", "string", "number", "Date");
        }

        public static string For(string tsTypeName, bool singleQuotes)
        {
            Requires.NotNull(tsTypeName, nameof(tsTypeName));
            
            switch (tsTypeName)
            {
                case "Object":
                    return "{}";
                case "boolean":
                    return "false";
                case "string":
                    return singleQuotes ? "''" : "\"\"";
                case "number":
                    return "0";
                case "Date":
                    return "new Date()";
                default:
                    return null;
            }
        }
    }
}