using System;
using System.Collections.Generic;
using System.Reflection;

namespace TypeGen.Core.Business
{
    internal interface IMetadataReader
    {
        TAttribute GetAttribute<TAttribute>(Type type) where TAttribute : Attribute;
        TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute;
        IEnumerable<TAttribute> GetAttributes<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute;
    }
}