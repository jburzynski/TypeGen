using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.Converters;

namespace TypeGen.Core.Services
{
    public class TypeService
    {
        /// <summary>
        /// Determines if a type has a TypeScript simple type representation
        /// </summary>
        /// <param name="type"></param>
        /// <returns>True if a corresponding TypeScript simple type exists; false otherwise.</returns>
        public bool IsTsSimpleType(Type type)
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
        /// <param name="typeNameConverters"></param>
        /// <returns></returns>
        public string GetTsTypeName(Type type, TypeNameConverterCollection typeNameConverters)
        {
            // handle simple types
            if (IsTsSimpleType(type))
            {
                return GetTsSimpleTypeName(type);
            }

            // handle collection types
            if (type.FullName != "System.String" && type.GetInterface("IEnumerable") != null)
            {
                Type elementType = GetTsCollectionElementType(type);
                if (elementType == null) throw new ApplicationException("TS collection element type is null");

                return GetTsTypeName(elementType, typeNameConverters) + "[]";
            }

            // handle custom types
            return typeNameConverters.Convert(type.Name, type);
        }

        /// <summary>
        /// Gets a type of a collection element from the given type.
        /// If a type is not an array type or does not contain the IEnumerable interface, null is returned.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type GetTsCollectionElementType(Type type)
        {
            // handle array types
            Type elementType = type.GetElementType();
            if (elementType != null)
            {
                return elementType;
            }

            // handle IEnumerable<>
            if (type.Name == "IEnumerable`1")
            {
                return type.GetGenericArguments()[0];
            }

            // handle types implementing IEnumerable<>
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType
                    && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return interfaceType.GetGenericArguments()[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets TypeScript type name for a simple type.
        /// Simple type must be one of: object, bool, string, int, float, double, decimal.
        /// </summary>
        /// <param name="type">one of: object, bool, string, int, float, double, decimal</param>
        /// <returns>TypeScript type name. Null if the passed type cannot be represented as a TypeScript simple type.</returns>
        public string GetTsSimpleTypeName(Type type)
        {
            switch (type.FullName)
            {
                case "System.Object":
                    return "Object";
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
