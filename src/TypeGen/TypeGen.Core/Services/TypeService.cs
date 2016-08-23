using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TypeGen.Core.Converters;
using TypeGen.Core.Extensions;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.Services
{
    /// <summary>
    /// Contains logic for retrieving information about types, relevant to generating TypeScript files.
    /// </summary>
    internal class TypeService
    {
        /// <summary>
        /// Determines if a type has a TypeScript simple type representation
        /// </summary>
        /// <param name="type"></param>
        /// <returns>True if a corresponding TypeScript simple type exists; false otherwise.</returns>
        public bool IsTsSimpleType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return new[]
            {
                "System.Object",
                "System.Boolean",
                "System.String",
                "System.Int32",
                "System.Int64",
                "System.Single",
                "System.Double",
                "System.Decimal"
            }.Contains(type.FullName);
        }

        /// <summary>
        /// Gets TypeScript type name for a simple type.
        /// Simple type must be one of: object, bool, string, int, long, float, double, decimal.
        /// </summary>
        /// <param name="type">one of: object, bool, string, int, long, float, double, decimal</param>
        /// <returns>TypeScript type name. Null if the passed type cannot be represented as a TypeScript simple type.</returns>
        public string GetTsSimpleTypeName(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            switch (type.FullName)
            {
                case "System.Object":
                    return "Object";
                case "System.Boolean":
                    return "boolean";
                case "System.String":
                    return "string";
                case "System.Int32":
                case "System.Int64":
                case "System.Single":
                case "System.Double":
                case "System.Decimal":
                    return "number";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Determines whether the type represents a TypeScript class
        /// </summary>
        /// <param name="type"></param>
        /// <returns>True if the type represents a TypeScript class; false otherwise</returns>
        /// <exception cref="ArgumentNullException">Thrown if the type is null</exception>
        public bool IsTsClass(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (!type.IsClass) return false;

            var exportAttribute = type.GetCustomAttribute<ExportAttribute>();
            return exportAttribute == null || exportAttribute is ExportTsClassAttribute;
        }

        /// <summary>
        /// Determines whether the type represents a TypeScript class
        /// </summary>
        /// <param name="type"></param>
        /// <returns>True is the type represents a TypeScript class; false otherwise</returns>
        /// <exception cref="ArgumentNullException">Thrown if the type is null</exception>
        public bool IsTsInterface(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (!type.IsClass) return false;

            var exportAttribute = type.GetCustomAttribute<ExportAttribute>();
            return exportAttribute is ExportTsInterfaceAttribute;
        }

        /// <summary>
        /// Gets MemberInfos of all members in a type that can be exported to TypeScript.
        /// Members marked with TsIgnore attribute are not included in the result.
        /// If the passed type is not a class type, empty enumeration is returned.
        /// </summary>
        /// <param name="type">Class type</param>
        /// <returns></returns>
        public IEnumerable<MemberInfo> GetTsExportableMembers(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!type.IsClass) return Enumerable.Empty<MemberInfo>();

            var fieldInfos = (IEnumerable<MemberInfo>) type.GetFields(BindingFlags.Instance | BindingFlags.Public)
                .WithoutTsIgnore();

            var propertyInfos = (IEnumerable<MemberInfo>) type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .WithoutTsIgnore();

            return fieldInfos.Union(propertyInfos);
        }

        /// <summary>
        /// Gets member's type.
        /// MemberInfo must be a PropertyInfo or a FieldInfo.
        /// </summary>
        /// <param name="memberInfo">PropertyInfo or FieldInfo</param>
        /// <returns></returns>
        public Type GetMemberType(MemberInfo memberInfo)
        {
            if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

            if (!memberInfo.Is<FieldInfo>() && !memberInfo.Is<PropertyInfo>())
            {
                throw new CoreException($"{memberInfo} must be either a FieldInfo or a PropertyInfo");
            }

            return memberInfo is PropertyInfo
                ? ToExportableType(((PropertyInfo)memberInfo).PropertyType)
                : ToExportableType(((FieldInfo)memberInfo).FieldType);
        }

        /// <summary>
        /// Determines if a type is a collection type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsCollectionType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return type.FullName != "System.String" && type.GetInterface("IEnumerable") != null;
        }

        /// <summary>
        /// Gets TypeScript type name for a type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeNameConverters"></param>
        /// <returns></returns>
        public string GetTsTypeName(Type type, TypeNameConverterCollection typeNameConverters)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (typeNameConverters == null) throw new ArgumentNullException(nameof(typeNameConverters));

            type = ToExportableType(type);

            // handle simple types
            if (IsTsSimpleType(type))
            {
                return GetTsSimpleTypeName(type);
            }

            // handle collection types
            if (IsCollectionType(type))
            {
                Type elementType = GetTsCollectionElementType(type);
                if (elementType == null) throw new CoreException("TS collection element type is null");

                return GetTsTypeName(elementType, typeNameConverters) + "[]";
            }

            // handle custom types
            return typeNameConverters.Convert(type.Name, type);
        }

        /// <summary>
        /// Gets a type of a collection element from the given type.
        /// If the passed type is not an array type or does not contain the IEnumerable interface, null is returned.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type GetTsCollectionElementType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

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
        /// Gets the type of the deepest element from a jagged collection of the given type.
        /// If the passed type is not an array type or does not contain the IEnumerable interface, the type itself is returned.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type GetFlatTsCollectionElementType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            while (true)
            {
                if (!IsCollectionType(type)) return type;
                type = GetTsCollectionElementType(type);
            }
        }

        /// <summary>
        /// Gets type dependencies related to generic type definition
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the type is null</exception>
        private IEnumerable<TypeDependencyInfo> GetGenericTypeDependencies(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (!type.IsGenericTypeDefinition) yield break;

            foreach (Type genericArgumentType in type.GetGenericArguments())
            {
                if (genericArgumentType.BaseType != null && genericArgumentType.BaseType != typeof(object))
                {
                    yield return new TypeDependencyInfo
                    {
                        Type = genericArgumentType.BaseType,
                        MemberAttributes = null
                    };
                }
            }
        }

        /// <summary>
        /// Gets the base type dependency for a type, if the base type exists
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the type is null</exception>
        private IEnumerable<TypeDependencyInfo> GetBaseTypeDependency(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            Type baseType = GetBaseType(type);
            if (baseType != null)
            {
                yield return new TypeDependencyInfo
                {
                    Type = baseType,
                    MemberAttributes = null
                };
            }
        }

        /// <summary>
        /// Gets type dependencies for the members inside a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the type is null</exception>
        private IEnumerable<TypeDependencyInfo> GetMemberTypeDependencies(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            IEnumerable<MemberInfo> memberInfos = GetTsExportableMembers(type);
            foreach (MemberInfo memberInfo in memberInfos)
            {
                Type memberType = GetMemberType(memberInfo);

                Type memberFlatType = IsCollectionType(memberType)
                    ? GetFlatTsCollectionElementType(memberType)
                    : memberType;

                if (!IsTsSimpleType(memberFlatType))
                {
                    yield return new TypeDependencyInfo
                    {
                        Type = memberFlatType,
                        MemberAttributes = memberInfo.GetCustomAttributes(typeof(Attribute), false) as Attribute[]
                    };
                }
            }
        }

        /// <summary>
        /// Gets all non-simple and non-collection types the given type depends on.
        /// Types of properties/fields marked with TsIgnoreAttribute will be omitted.
        /// Returns an empty array if no dependencies were detected.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the type is null</exception>
        public IEnumerable<TypeDependencyInfo> GetTypeDependencies(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!type.IsClass) return Enumerable.Empty<TypeDependencyInfo>();

            type = ToExportableType(type);

            return GetGenericTypeDependencies(type)
                .Concat(GetBaseTypeDependency(type)
                .Concat(GetMemberTypeDependencies(type)));
        }

        /// <summary>
        /// Converts a type to a 'TS-exportable' type.
        /// If the type is nullable, returns the underlying type.
        /// Otherwise, returns the passed type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Type ToExportableType(Type type)
        {
            Type nullableUnderlyingType = Nullable.GetUnderlyingType(type);
            return nullableUnderlyingType ?? type;
        }

        /// <summary>
        /// Gets custom base type for a class type.
        /// If no custom base type exists, null is returned.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if type is null</exception>
        /// <exception cref="CoreException">Thrown if the type is not a class type or inheritance chain cannot be represented in TypeScript</exception>
        public Type GetBaseType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!type.IsClass) throw new CoreException($"Type '{type.FullName}' should be a class type");

            Type baseType = type.BaseType;
            if (baseType == null || baseType == typeof(object)) return null;

            if (IsTsClass(type) && IsTsInterface(baseType)) throw new CoreException($"Attempted to generate class '{type.FullName}' which extends an interface '{baseType.FullName}', which is not a valid inheritance chain in TypeScript");

            return baseType;
        }
    }
}
