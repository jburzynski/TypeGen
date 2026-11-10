using System;
using System.Collections.Generic;
using System.Reflection;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Metadata
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

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(Type type) where TAttribute : Attribute
        {
            Requires.NotNull(type, nameof(type));
            return type.GetTypeInfo().GetCustomAttributes(typeof(TAttribute), false) as TAttribute[];
        }

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));
            return memberInfo.GetCustomAttributes(typeof(TAttribute), false) as TAttribute[];
        }

        public IEnumerable<object> GetAttributes(Type type)
        {
            Requires.NotNull(type, nameof(type));
            return type.GetTypeInfo().GetCustomAttributes(false);
        }

        public IEnumerable<object> GetAttributes(MemberInfo memberInfo)
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));
            try
            {
                return memberInfo.GetCustomAttributes(false);
            }
            catch (Exception ex)
            {
                return [];
            }
        }
    }
}