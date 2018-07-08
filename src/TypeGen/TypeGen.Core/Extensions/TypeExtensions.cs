using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.Extensions
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Checks if a type is marked with an ExportTs... attribute
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasExportAttribute(this Type type)
        {
            return type.GetTypeInfo().GetCustomAttribute<ExportTsClassAttribute>() != null ||
                   type.GetTypeInfo().GetCustomAttribute<ExportTsInterfaceAttribute>() != null ||
                   type.GetTypeInfo().GetCustomAttribute<ExportTsEnumAttribute>() != null;
        }

        /// <summary>
        /// Gets all types marked with ExportTs... attributes
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetExportMarkedTypes(this IEnumerable<Type> types)
        {
            return types.Where(t => t.HasExportAttribute());
        }

        /// <summary>
        /// Removes members marked with TsIgnore attribute
        /// </summary>
        /// <param name="memberInfos"></param>
        /// <returns></returns>
        public static IEnumerable<T> WithoutTsIgnore<T>(this IEnumerable<T> memberInfos) where T : MemberInfo
        {
            return memberInfos.Where(i => i.GetCustomAttribute<TsIgnoreAttribute>() == null);
        }

        /// <summary>
        /// Filters members for TypeScript export
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberInfos"></param>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> WithMembersFilter(this IEnumerable<FieldInfo> memberInfos)
        {
            return memberInfos.Where(i => i.IsPublic && !i.IsStatic);
        }

        /// <summary>
        /// Filters members for TypeScript export
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberInfos"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> WithMembersFilter(this IEnumerable<PropertyInfo> memberInfos)
        {
            return memberInfos.Where(i => i.CanRead && !i.GetMethod.IsStatic);
        }

        /// <summary>
        /// Maps an enumerable to an enumerable of the elements' type names
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetTypeNames(this IEnumerable<object> enumerable)
        {
            return enumerable
                .Select(c => c.GetType().Name);
        }
    }
}
