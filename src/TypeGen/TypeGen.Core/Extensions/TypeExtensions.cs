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
        /// Shim for .NET Framework Type.GetInterface
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type GetInterface(this Type type, string name)
        {
            return type.GetTypeInfo().ImplementedInterfaces
                .FirstOrDefault(i => i.FullName.Split('[')[0] == name || i.Name.Split('[')[0] == name);
        }

        /// <summary>
        /// Shim for .NET Framework Type.GetGenericArguments
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type[] GetGenericArguments(this Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();

            return typeInfo.GenericTypeArguments
                .Concat(typeInfo.GenericTypeParameters)
                .ToArray();
        }

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
    }
}
