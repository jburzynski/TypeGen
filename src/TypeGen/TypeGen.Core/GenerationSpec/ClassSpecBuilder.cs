using System;
using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.GenerationSpec
{
    public class ClassSpecBuilder<T>
    {
        private readonly ClassSpec _spec;

        private readonly T _instance;
        private string _activeMemberName;

        internal ClassSpecBuilder(ClassSpec spec)
        {
            _spec = spec;
            _instance = default;
        }

        private void AddActiveMemberAttribute(Attribute attribute) => _spec.MemberAttributes[_activeMemberName].Add(attribute);
        private void AddTypeAttribute(Attribute attribute) => _spec.AdditionalAttributes.Add(attribute);
        
        public ClassSpecBuilder<T> Member(Func<T, string> memberNameFunc)
        {
            return Member(memberNameFunc(_instance));
        }
        
        public ClassSpecBuilder<T> Member(string memberName)
        {
            _activeMemberName = memberName;
            _spec.AddMember(_activeMemberName);
            return this;
        }

        public ClassSpecBuilder<T> CustomBase(string @base, string importType = null, string originalTypeName = null)
        {
            AddTypeAttribute(new TsCustomBaseAttribute(@base, importType, originalTypeName));
            return this;
        }
        
        public ClassSpecBuilder<T> DefaultTypeOutput(string outputDir)
        {
            AddActiveMemberAttribute(new TsDefaultTypeOutputAttribute(outputDir));
            return this;
        }
        
        public ClassSpecBuilder<T> DefaultValue(string defaultValue)
        {
            AddActiveMemberAttribute(new TsDefaultValueAttribute(defaultValue));
            return this;
        }
        
        public ClassSpecBuilder<T> Ignore()
        {
            AddActiveMemberAttribute(new TsIgnoreAttribute());
            return this;
        }
        
        public ClassSpecBuilder<T> IgnoreBase()
        {
            AddTypeAttribute(new TsIgnoreBaseAttribute());
            return this;
        }
        
        public ClassSpecBuilder<T> MemberName(string name)
        {
            AddActiveMemberAttribute(new TsMemberNameAttribute(name));
            return this;
        }
        
        public ClassSpecBuilder<T> NotNull()
        {
            AddActiveMemberAttribute(new TsNotNullAttribute());
            return this;
        }
        
        public ClassSpecBuilder<T> NotUndefined()
        {
            AddActiveMemberAttribute(new TsNotUndefinedAttribute());
            return this;
        }
        
        public ClassSpecBuilder<T> Null()
        {
            AddActiveMemberAttribute(new TsNullAttribute());
            return this;
        }
        
        public ClassSpecBuilder<T> Optional()
        {
            AddActiveMemberAttribute(new TsOptionalAttribute());
            return this;
        }
        
        public ClassSpecBuilder<T> Type(string typeName, string importPath = null, string originalTypeName = null)
        {
            AddActiveMemberAttribute(new TsTypeAttribute(typeName, importPath, originalTypeName));
            return this;
        }
        
        public ClassSpecBuilder<T> Undefined()
        {
            AddActiveMemberAttribute(new TsUndefinedAttribute());
            return this;
        }
    }
}