using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.Extensions
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Removes members marked with TsIgnore attribute
        /// </summary>
        /// <param name="memberInfos"></param>
        /// <returns></returns>
        public static IEnumerable<T> WithoutTsIgnore<T>(this IEnumerable<T> memberInfos) where T : MemberInfo
        {
            return memberInfos.Where(i => AttributeExtensions.GetCustomAttribute<TsIgnoreAttribute>((MemberInfo) i) == null);
        }
    }
}
