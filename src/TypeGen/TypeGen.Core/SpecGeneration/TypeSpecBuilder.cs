using System;

namespace TypeGen.Core.SpecGeneration
{
    public abstract class TypeSpecBuilder<T, TDerived> where TDerived : TypeSpecBuilder<T, TDerived>
    {
        internal readonly TypeSpec _spec;
        protected internal readonly T _instance;
        protected internal string _activeMemberName;

        internal TypeSpecBuilder(TypeSpec spec)
        {
            _spec = spec;
            _instance = default;
        }

        protected internal void AddActiveMemberAttribute(Attribute attribute) => _spec.MemberAttributes[_activeMemberName].Add(attribute);
        protected internal void AddTypeAttribute(Attribute attribute) => _spec.AdditionalAttributes.Add(attribute);
        
        public TDerived Member(string memberName)
        {
            _activeMemberName = memberName;
            _spec.AddMember(_activeMemberName);
            
            return this as TDerived;
        }
    }
}