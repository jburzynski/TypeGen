using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Business
{
    internal class GenerationSpecTypeMetadataReader : IMetadataReader
    {
        private readonly GenerationSpec _spec;

        public GenerationSpecTypeMetadataReader(GenerationSpec spec)
        {
            _spec = spec;
        }

        public TAttribute GetAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            Requires.NotNull(type, nameof(type));

            if (!_spec.TypeSpecs.ContainsKey(type)) return null;
            
            if (_spec.TypeSpecs[type].ExportAttribute is TAttribute attribute) return attribute;
            return _spec.TypeSpecs[type].AdditionalAttributes.FirstOrDefault(a => a is TAttribute) as TAttribute;
        }

        public TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));

            if (!_spec.TypeSpecs.ContainsKey(memberInfo.DeclaringType) ||
                !_spec.TypeSpecs[memberInfo.DeclaringType].MemberAttributes.ContainsKey(memberInfo.Name))
            {
                return null;
            }
            
            return _spec.TypeSpecs[memberInfo.DeclaringType]
                .MemberAttributes[memberInfo.Name]
                .FirstOrDefault(a => a is TAttribute) as TAttribute;
        }

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));
            
            if (!_spec.TypeSpecs.ContainsKey(memberInfo.DeclaringType) ||
                !_spec.TypeSpecs[memberInfo.DeclaringType].MemberAttributes.ContainsKey(memberInfo.Name))
            {
                return null;
            }
            
            return _spec.TypeSpecs[memberInfo.DeclaringType]
                    .MemberAttributes[memberInfo.Name]
                    .Where(a => a is TAttribute)
                as IEnumerable<TAttribute>;
        }
    }
}