using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Metadata
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

            if (!_spec.TypeSpecs.ContainsKey(type)) return null;
            
            if (_spec.TypeSpecs[type].ExportAttribute.GetType() == typeof(TAttribute)) return _spec.TypeSpecs[type].ExportAttribute as TAttribute;
            return _spec.TypeSpecs[type].AdditionalAttributes.FirstOrDefault(a => a.GetType() == typeof(TAttribute)) as TAttribute;
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
                .FirstOrDefault(a => a.GetType() == typeof(TAttribute)) as TAttribute;
        }
        
        public IEnumerable<TAttribute> GetAttributes<TAttribute>(Type type) where TAttribute : Attribute
        {
            Requires.NotNull(type, nameof(type));
            
            return GetAttributes(type)
                    .Where(a => a.GetType() == typeof(TAttribute))
                as IEnumerable<TAttribute>;
        }

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));
            
            return GetAttributes(memberInfo)
                    .Where(a => a.GetType() == typeof(TAttribute))
                as IEnumerable<TAttribute>;
        }

        public IEnumerable<object> GetAttributes(Type type)
        {
            Requires.NotNull(type, nameof(type));
            
            if (!_spec.TypeSpecs.ContainsKey(type)) return Enumerable.Empty<object>();

            return new[] { _spec.TypeSpecs[type].ExportAttribute }
                .Concat(_spec.TypeSpecs[type].AdditionalAttributes)
                .ToList();
        }
        
        public IEnumerable<object> GetAttributes(MemberInfo memberInfo)
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));
            
            if (!_spec.TypeSpecs.ContainsKey(memberInfo.DeclaringType) ||
                !_spec.TypeSpecs[memberInfo.DeclaringType].MemberAttributes.ContainsKey(memberInfo.Name))
            {
                return Enumerable.Empty<object>();
            }

            return _spec.TypeSpecs[memberInfo.DeclaringType]
                .MemberAttributes[memberInfo.Name];
        }
    }
}