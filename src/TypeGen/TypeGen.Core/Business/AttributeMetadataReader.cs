using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.TypeAnnotations;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Business
{
    internal class AttributeMetadataReader : IMetadataReader
    {
        public TAttribute GetAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            Requires.NotNull(type, nameof(type));
            return type.GetTypeInfo().GetCustomAttribute<TAttribute>();
        }

        public TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));
            return memberInfo.GetCustomAttribute<TAttribute>();
        }

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));
            return memberInfo.GetCustomAttributes(typeof(TAttribute), false) as TAttribute[];
        }
    }
}