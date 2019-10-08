using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.Metadata;
using TypeGen.Core.TypeAnnotations;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Extensions
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Checks if a type is marked with an ExportTs... attribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static bool HasExportAttribute(this Type type, IMetadataReader reader)
        {
            Requires.NotNull(type, nameof(type));
            Requires.NotNull(reader, nameof(reader));
            
            return reader.GetAttribute<ExportTsClassAttribute>(type) != null ||
                   reader.GetAttribute<ExportTsInterfaceAttribute>(type) != null ||
                   reader.GetAttribute<ExportTsEnumAttribute>(type) != null;
        }

        /// <summary>
        /// Gets all types marked with ExportTs... attributes
        /// </summary>
        /// <param name="types"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetExportMarkedTypes(this IEnumerable<Type> types, IMetadataReader reader)
        {
            Requires.NotNull(types, nameof(types));
            Requires.NotNull(reader, nameof(reader));
            
            return types.Where(t => t.HasExportAttribute(reader));
        }

        /// <summary>
        /// Removes members marked with TsIgnore attribute
        /// </summary>
        /// <param name="memberInfos"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<T> WithoutTsIgnore<T>(this IEnumerable<T> memberInfos, IMetadataReader reader) where T : MemberInfo
        {
            Requires.NotNull(memberInfos, nameof(memberInfos));
            Requires.NotNull(reader, nameof(reader));
            
            return memberInfos.Where(i => reader.GetAttribute<TsIgnoreAttribute>(i) == null);
        }

        /// <summary>
        /// Filters members for TypeScript export
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberInfos"></param>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> WithMembersFilter(this IEnumerable<FieldInfo> memberInfos)
        {
            Requires.NotNull(memberInfos, nameof(memberInfos));
            return memberInfos.Where(i => i.IsPublic);
        }

        /// <summary>
        /// Filters members for TypeScript export
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberInfos"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> WithMembersFilter(this IEnumerable<PropertyInfo> memberInfos)
        {
            Requires.NotNull(memberInfos, nameof(memberInfos));
            return memberInfos.Where(i => i.GetMethod.IsPublic);
        }

        /// <summary>
        /// Checks if a property or field is static
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static bool IsStatic(this MemberInfo memberInfo)
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));

            if (memberInfo is FieldInfo fieldInfo) return fieldInfo.IsStatic;
            if (memberInfo is PropertyInfo propertyInfo) return propertyInfo.GetMethod.IsStatic;

            return false;
        }

        /// <summary>
        /// Maps an enumerable to an enumerable of the elements' type names
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetTypeNames(this IEnumerable<object> enumerable)
        {
            Requires.NotNull(enumerable, nameof(enumerable));
            
            return enumerable
                .Select(c => c.GetType().Name);
        }
        
        /// <summary>
        /// Shim for Type.GetInterface
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceName"></param>
        /// <returns></returns>
        public static Type GetInterface(this Type type, string interfaceName)
        {
            Requires.NotNull(type, nameof(type));
            Requires.NotNullOrEmpty(interfaceName, nameof(interfaceName));
            
            return type.GetInterfaces()
                .FirstOrDefault(i => i.Name == interfaceName || i.FullName == interfaceName);
        }

        /// <summary>
        /// Gets MemberInfos of all members in a type that can be exported to TypeScript.
        /// Members marked with TsIgnore attribute are not included in the result.
        /// If the passed type is not a class type, empty enumeration is returned.
        /// </summary>
        /// <param name="type">Class type</param>
        /// <param name="metadataReader"></param>
        /// <param name="withoutTsIgnore"></param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetTsExportableMembers(this Type type, IMetadataReader metadataReader, bool withoutTsIgnore = true)
        {
            Requires.NotNull(type, nameof(type));
            TypeInfo typeInfo = type.GetTypeInfo();

            if (!typeInfo.IsClass && !typeInfo.IsInterface) return Enumerable.Empty<MemberInfo>();

            var fieldInfos = (IEnumerable<MemberInfo>) typeInfo.DeclaredFields
                .WithMembersFilter();
            
            var propertyInfos = (IEnumerable<MemberInfo>) typeInfo.DeclaredProperties
                .WithMembersFilter();

            if (withoutTsIgnore)
            {
                fieldInfos = fieldInfos.WithoutTsIgnore(metadataReader);
                propertyInfos = propertyInfos.WithoutTsIgnore(metadataReader);
            }  

            return fieldInfos.Union(propertyInfos);
        }
    }
}
