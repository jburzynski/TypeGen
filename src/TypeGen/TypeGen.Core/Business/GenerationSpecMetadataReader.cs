using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Business
{
    internal class GenerationSpecMetadataReader : IMetadataReader
    {
        private readonly GenerationSpec _spec;

        public GenerationSpecMetadataReader(GenerationSpec spec)
        {
            _spec = spec;
        }

        public TAttribute GetAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            Requires.NotNull(type, nameof(type));
            
            if (_spec.TypeSpecs[type].ExportAttribute is TAttribute attribute) return attribute;
            return _spec.TypeSpecs[type].AdditionalAttributes.FirstOrDefault(a => a is TAttribute) as TAttribute;
        }

        public TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));
            
            return _spec.TypeSpecs[memberInfo.DeclaringType]
                .MemberAttributes[memberInfo.Name]
                .FirstOrDefault(a => a is TAttribute) as TAttribute;
        }

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));
            
            return _spec.TypeSpecs[memberInfo.DeclaringType]
                    .MemberAttributes[memberInfo.Name]
                    .Where(a => a is TAttribute)
                as IEnumerable<TAttribute>;
        }
    }
}