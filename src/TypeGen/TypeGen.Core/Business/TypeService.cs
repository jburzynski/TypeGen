using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using TypeGen.Core.Converters;
using TypeGen.Core.Extensions;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.Business
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

            return GetTsSimpleTypeName(type) != null;
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
                case "System.DateTime":
                    return "Date";
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

            var fieldInfos = (IEnumerable<MemberInfo>) type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .WithoutTsIgnore();

            var propertyInfos = (IEnumerable<MemberInfo>) type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
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
        public bool IsCollectionType(Type type)
        {
            return type.FullName != "System.String" // not a string
                && !IsDictionaryType(type) // not a dictionary
                && type.GetInterface("IEnumerable") != null; // implements IEnumerable
        }

        /// <summary>
        /// Determines if a type is a dictionary type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsDictionaryType(Type type)
        {
            return type.GetInterface("System.Collections.Generic.IDictionary`2") != null
                   || (type.FullName != null && type.FullName.StartsWith("System.Collections.Generic.IDictionary`2"));
        }

        /// <summary>
        /// Determines if a type is a user-defined generic type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsCustomGenericType(Type type)
        {
            return type.IsGenericType && !IsDictionaryType(type) && !IsCollectionType(type);
        }

        /// <summary>
        /// Gets TypeScript type name for a member
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="typeNameConverters"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when member or typeNameConverters is null</exception>
        public string GetTsTypeName(MemberInfo memberInfo, TypeNameConverterCollection typeNameConverters)
        {
            if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

            // special case - member is dynamic

            if (memberInfo.GetCustomAttribute<DynamicAttribute>() != null)
            {
                return "any";
            }

            // otherwise, resolve by type

            Type type = GetMemberType(memberInfo);
            return GetTsTypeName(type, typeNameConverters);
        }

        /// <summary>
        /// Gets TypeScript type name for a type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeNameConverters"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when type or typeNameConverters is null</exception>
        /// <exception cref="CoreException">Thrown when collection element type for the passed type is null (occurs only if the passed type is a collection type)</exception>
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
                return GetTsCollectionTypeName(type, typeNameConverters);
            }

            // handle dictionaries
            if (IsDictionaryType(type))
            {
                return GetTsDictionaryTypeName(type, typeNameConverters);
            }

            // handle custom generic types
            if (IsCustomGenericType(type))
            {
                return GetGenericTsTypeName(type, typeNameConverters);
            }

            // handle custom types & generic parameters
            string typeNameNoArity = type.Name.RemoveTypeArity();
            return type.IsGenericParameter ? typeNameNoArity : typeNameConverters.Convert(typeNameNoArity, type);
        }

        /// <summary>
        /// Gets TypeScript type name for a dictionary type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeNameConverters"></param>
        /// <returns></returns>
        private string GetTsDictionaryTypeName(Type type, TypeNameConverterCollection typeNameConverters)
        {
            Type interfaceType = type.GetInterface("System.Collections.Generic.IDictionary`2") ?? type;
            Type keyType = interfaceType.GetGenericArguments()[0];
            Type valueType = interfaceType.GetGenericArguments()[1];

            string keyTypeName = GetTsTypeName(keyType, typeNameConverters);
            string valueTypeName = GetTsTypeName(valueType, typeNameConverters);

            if (!keyTypeName.In("number", "string"))
            {
                throw new CoreException($"Error when determining TypeScript type for C# type '{type.FullName}':" +
                                        " TypeScript dictionary key type must be either 'number' or 'string'");
            }

            return $"{{ [key: {keyTypeName}]: {valueTypeName}; }}";
        }

        /// <summary>
        /// Gets TypeScript type name for a collection type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeNameConverters"></param>
        /// <returns></returns>
        private string GetTsCollectionTypeName(Type type, TypeNameConverterCollection typeNameConverters)
        {
            Type elementType = GetTsCollectionElementType(type);
            return GetTsTypeName(elementType, typeNameConverters) + "[]";
        }

        /// <summary>
        /// Gets TypeScript type name for a generic type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeNameConverters"></param>
        /// <returns></returns>
        private string GetGenericTsTypeName(Type type, TypeNameConverterCollection typeNameConverters)
        {
            return type.IsGenericTypeDefinition
                ? GetGenericDefinitionTsTypeName(type, typeNameConverters)
                : GetGenericNonDefinitionTsTypeName(type, typeNameConverters);
        }

        /// <summary>
        /// Gets TypeScript type name for a generic definition type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeNameConverters"></param>
        /// <returns></returns>
        private string GetGenericDefinitionTsTypeName(Type type, TypeNameConverterCollection typeNameConverters)
        {
            Type[] genericArguments = type.GetGenericArguments();

            string[] genericArgumentNames = (from t in genericArguments
                                             select t.BaseType != null && t.BaseType != typeof(object)
                                                 ? $"{t.Name} extends {GetTsTypeName(t.BaseType, typeNameConverters)}"
                                                 : t.Name)
                                            .ToArray();

            string typeName = type.Name.RemoveTypeArity();
            string genericArgumentDef = string.Join(", ", genericArgumentNames);
            return $"{typeNameConverters.Convert(typeName, type)}<{genericArgumentDef}>";
        }

        /// <summary>
        /// Gets TypeScript type name for a generic (not generic definition) type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeNameConverters"></param>
        /// <returns></returns>
        private string GetGenericNonDefinitionTsTypeName(Type type, TypeNameConverterCollection typeNameConverters)
        {
            string[] genericArgumentNames = type.GetGenericArguments()
                .Select(t => t.IsGenericParameter ? t.Name : GetTsTypeName(t, typeNameConverters))
                .ToArray();

            string typeName = type.Name.RemoveTypeArity();
            string genericArgumentDef = string.Join(", ", genericArgumentNames);
            return $"{typeNameConverters.Convert(typeName, type)}<{genericArgumentDef}>";
        }

        /// <summary>
        /// Gets a type of a collection element from the given type.
        /// If the passed type is not an array type or does not contain the IEnumerable interface, null is returned.
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
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return interfaceType.GetGenericArguments()[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the type of the deepest element from a jagged collection of the given type.
        /// If the passed type is not an array type or does not implement IEnumerable interface, the type itself is returned.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type GetFlatType(Type type)
        {
            while (true)
            {
                if (!IsCollectionType(type)) return type;
                type = GetTsCollectionElementType(type);
            }
        }

        /// <summary>
        /// Converts a type to a 'TS-exportable' type.
        /// If the type is nullable, returns the underlying type.
        /// Otherwise, returns the passed type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type ToExportableType(Type type)
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
