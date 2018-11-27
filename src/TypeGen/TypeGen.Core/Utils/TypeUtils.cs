using System;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Utils
{
    public class TypeUtils
    {
        /// <summary>
        /// Determines if a type has a TypeScript simple type representation
        /// </summary>
        /// <param name="type"></param>
        /// <returns>True if a corresponding TypeScript simple type exists; false otherwise.</returns>
        public static bool IsTsSimpleType(Type type)
        {
            Requires.NotNull(type, nameof(type));

            return GetTsSimpleTypeName(type) != null;
        }

        /// <summary>
        /// Gets TypeScript type name for a simple type.
        /// Simple type must be one of: object, bool, string, int, long, float, double, decimal.
        /// </summary>
        /// <param name="type">one of: object, bool, string, int, long, float, double, decimal</param>
        /// <returns>TypeScript type name. Null if the passed type cannot be represented as a TypeScript simple type.</returns>
        public static string GetTsSimpleTypeName(Type type)
        {
            Requires.NotNull(type, nameof(type));

            switch (type.FullName)
            {
                case "System.Object":
                    return "Object";
                case "System.Boolean":
                    return "boolean";
                case "System.Char":
                case "System.String":
                case "System.Guid":
                    return "string";
                case "System.SByte":
                case "System.Byte":
                case "System.Int16":
                case "System.UInt16":
                case "System.Int32":
                case "System.UInt32":
                case "System.Int64":
                case "System.UInt64":
                case "System.Single":
                case "System.Double":
                case "System.Decimal":
                    return "number";
                case "System.DateTime":
                case "System.DateTimeOffset":
                case "System.TimeSpan":
                    return "Date";
                default:
                    return null;
            }
        }

        public static string StripOptionalAndTypeUnion(string tsTypeName) => tsTypeName.Split('?', '|')[0].Trim();
    }
}