using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.GenerationSpec;
using TypeGen.Core.TypeAnnotations;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Business
{
    internal class MetadataReader : IMetadataReader
    {
        public GenerationSpec.GenerationSpec Spec { get; set; }
        
        public TAttribute GetAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            Requires.NotNull(type, nameof(type));

            if (Spec == null) return type.GetTypeInfo().GetCustomAttribute<TAttribute>();
            
            if (Spec.TypeSpecs[type].ExportAttribute is TAttribute attribute) return attribute;
            return Spec.TypeSpecs[type].AdditionalAttributes.FirstOrDefault(a => a is TAttribute) as TAttribute;

        }

        public TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));

            if (Spec == null) return memberInfo.GetCustomAttribute<TAttribute>();
            
            return Spec.TypeSpecs[memberInfo.DeclaringType]
                .MemberAttributes[memberInfo.Name]
                .FirstOrDefault(a => a is TAttribute) as TAttribute;
        }

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));
            
            if (Spec == null) return memberInfo.GetCustomAttributes(typeof(TAttribute), false) as TAttribute[];

            return Spec.TypeSpecs[memberInfo.DeclaringType]
                .MemberAttributes[memberInfo.Name]
                .Where(a => a is TAttribute)
                as IEnumerable<TAttribute>;
        }
    }
}