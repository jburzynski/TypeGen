using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TypeGen.Core
{
    public static class AttributeExtensions
    {
        /// <summary>
        /// Shim for .NET 4.5 GetCustomAttribute
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this Type type) where TAttribute: Attribute
        {
            object[] attrib = type.GetCustomAttributes(typeof (TAttribute), false);

            if (attrib.Length == 0) return null;
            if (attrib.Length == 1) return (TAttribute)attrib[0];

            throw new AmbiguousMatchException($"GetCustomAttribute<{typeof(TAttribute).FullName}>");
        }

        /// <summary>
        /// Shim for .NET 4.5 GetCustomAttribute
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
        {
            object[] attrib = memberInfo.GetCustomAttributes(typeof(TAttribute), false);

            if (attrib.Length == 0) return null;
            if (attrib.Length == 1) return (TAttribute)attrib[0];

            throw new AmbiguousMatchException($"GetCustomAttribute<{typeof(TAttribute).FullName}>");
        }
    }
}
