using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Core
{
    /// <summary>
    /// Utility class
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Gets embedded resource as string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetEmbeddedResource(string name)
        {
            using (Stream stream = typeof (Utilities).Assembly.GetManifestResourceStream(name))
            {
                var contentBytes = new byte[stream.Length];
                stream.Read(contentBytes, 0, (int)stream.Length);
                return Encoding.ASCII.GetString(contentBytes);
            }
        }

        /// <summary>
        /// Determines if a type has a TypeScript simple type representation
        /// </summary>
        /// <param name="type"></param>
        /// <returns>True if a corresponding TypeScript simple type exists; false otherwise.</returns>
        public static bool IsTsSimpleType(Type type)
        {
            return new[]
            {
                "System.Boolean",
                "System.String",
                "System.Int32",
                "System.Single",
                "System.Double",
                "System.Decimal"
            }.Contains(type.FullName);
        }

        /// <summary>
        /// Gets TypeScript type name for a type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTsTypeName(Type type)
        {
            return IsTsSimpleType(type) ? GetTsSimpleTypeName(type) : type.Name;
        }

        /// <summary>
        /// Gets TypeScript type name for a simple type.
        /// Simple type must be one of: bool, string, int, float, double, decimal.
        /// </summary>
        /// <param name="type">one of: bool, string, int, float, double, decimal</param>
        /// <returns>TypeScript type name. Null if the passed type cannot be represented as a TypeScript simple type.</returns>
        private static string GetTsSimpleTypeName(Type type)
        {
            switch (type.FullName)
            {
                case "System.Boolean":
                    return "boolean";
                case "System.String":
                    return "string";
                case "System.Int32":
                case "System.Single":
                case "System.Double":
                case "System.Decimal":
                    return "number";
                default:
                    return null;
            }
        }
    }
}
