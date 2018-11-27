using System;
using System.Collections.Generic;
using TypeGen.Core.Business;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Utils
{
    /// <summary>
    /// Determines empty values for types
    /// </summary>
    internal class EmptyValue
    {
        public static bool ExistsFor(Type type)
        {
            Requires.NotNull(type, nameof(type));
            
            return TypeUtils.GetTsSimpleTypeName(type) != null;
        }

        public static string For(Type type, bool singleQuotes)
        {
            Requires.NotNull(type, nameof(type));
            
            string tsSimpleTypeName = TypeUtils.GetTsSimpleTypeName(type);
            switch (tsSimpleTypeName)
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