using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Removes members marked with TsIgnore attribute
        /// </summary>
        /// <param name="memberInfos"></param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> WithoutTsIgnore(this IEnumerable<MemberInfo> memberInfos)
        {
            return memberInfos.Where(i => i.GetCustomAttribute<TsIgnoreAttribute>() == null);
        }
    }
}
