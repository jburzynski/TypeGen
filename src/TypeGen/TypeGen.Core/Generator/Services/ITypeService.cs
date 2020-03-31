using System;
using System.Collections.Generic;
using System.Reflection;

namespace TypeGen.Core.Generator.Services
{
    internal interface ITypeService
    {
        /// <summary>
        /// Determines if a type has a TypeScript simple type representation
        /// </summary>
        /// <param name="type"></param>
        /// <returns>True if a corresponding TypeScript simple type exists; false otherwise.</returns>
        bool IsTsSimpleType(Type type);

        /// <summary>
        /// Gets TypeScript type name for a simple type.
        /// Simple type must be one of: object, bool, string, int, long, float, double, decimal; or any type specified in GeneratorOptions.CustomMappings.
        /// </summary>
        /// <param name="type">one of: object, bool, string, int, long, float, double, decimal; or any type specified in GeneratorOptions.CustomMappings</param>
        /// <returns>TypeScript type name. Null if the passed type cannot be represented as a TypeScript simple type.</returns>
        string GetTsSimpleTypeName(Type type);
        
        /// <summary>
        /// Determines whether the type represents a TypeScript class
        /// </summary>
        /// <param name="type"></param>
        /// <returns>True if the type represents a TypeScript class; false otherwise</returns>
        /// <exception cref="ArgumentNullException">Thrown if the type is null</exception>
        bool IsTsClass(Type type);

        /// <summary>
        /// Determines whether the type represents a TypeScript class
        /// </summary>
        /// <param name="type"></param>
        /// <returns>True is the type represents a TypeScript class; false otherwise</returns>
        /// <exception cref="ArgumentNullException">Thrown if the type is null</exception>
        bool IsTsInterface(Type type);

        /// <summary>
        /// Gets member's type.
        /// MemberInfo must be a PropertyInfo or a FieldInfo.
        /// </summary>
        /// <param name="memberInfo">PropertyInfo or FieldInfo</param>
        /// <returns></returns>
        Type GetMemberType(MemberInfo memberInfo);

        /// <summary>
        /// Determines if a type is a collection type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsCollectionType(Type type);

        /// <summary>
        /// Determines if a type is a dictionary type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsDictionaryType(Type type);

        /// <summary>
        /// Determines if a type is a user-defined generic type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsCustomGenericType(Type type);

        /// <summary>
        /// Gets TypeScript type name for a type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="forTypeDeclaration"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when type or typeNameConverters is null</exception>
        /// <exception cref="CoreException">Thrown when collection element type for the passed type is null (occurs only if the passed type is a collection type)</exception>
        string GetTsTypeName(Type type, bool forTypeDeclaration = false);

        /// <summary>
        /// Gets the TypeScript type name to generate for a member
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        string GetTsTypeName(MemberInfo memberInfo);

        /// <summary>
        /// Gets the type of the deepest element from a jagged collection of the given type.
        /// If the passed type is not an array type or does not implement IEnumerable interface, the type itself is returned.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Type GetFlatType(Type type);

        /// <summary>
        /// Converts a type to a 'TS-exportable' type.
        /// If the type is nullable, returns the underlying type.
        /// Otherwise, returns the passed type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Type StripNullable(Type type);

        /// <summary>
        /// Gets custom base type for a class type.
        /// If no custom base type exists, null is returned.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if type is null</exception>
        /// <exception cref="CoreException">Thrown if the type is not a class type or inheritance chain cannot be represented in TypeScript</exception>
        Type GetBaseType(Type type);

        /// <summary>
        /// Determines whether to use default export in TypeScript for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool UseDefaultExport(Type type);

        IEnumerable<string> GetTypeUnions(MemberInfo memberInfo);
        IEnumerable<Type> GetInterfaces(Type type);
    }
}