using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                .FirstOrDefault(i => i.FullName == name || i.Name == name);
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
    }
}
