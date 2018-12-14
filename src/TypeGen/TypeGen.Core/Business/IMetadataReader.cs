using System;
using System.Collections.Generic;
using System.Reflection;
using TypeGen.Core.GenerationSpec;

namespace TypeGen.Core.Business
{
    internal interface IMetadataReader
    {
        GenerationSpec.GenerationSpec Spec { get; set; }
        
        TAttribute GetAttribute<TAttribute>(Type type) where TAttribute : Attribute;
        TAttribute GetAttribute<TAttribute>(MemberInfo member) where TAttribute : Attribute;
        IEnumerable<TAttribute> GetAttributes<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute;
    }
}