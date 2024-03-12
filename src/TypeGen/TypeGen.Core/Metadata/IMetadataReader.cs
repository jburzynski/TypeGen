using System;
using System.Collections.Generic;
using System.Reflection;

namespace TypeGen.Core.Metadata
{
    public interface IMetadataReader
    {
        TAttribute GetAttribute<TAttribute>(Type type) where TAttribute : Attribute;
        TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute;
        IEnumerable<TAttribute> GetAttributes<TAttribute>(Type type) where TAttribute : Attribute;
        IEnumerable<TAttribute> GetAttributes<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute;
        IEnumerable<object> GetAttributes(Type type);
        IEnumerable<object> GetAttributes(MemberInfo memberInfo);
    }
}