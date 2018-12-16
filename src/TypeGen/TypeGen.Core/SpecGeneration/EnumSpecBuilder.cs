using System;

namespace TypeGen.Core.SpecGeneration
{
    public class EnumSpecBuilder<T> : TypeSpecBuilder<T>
    {
        internal EnumSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
        
        public EnumSpecBuilder<T> Member(Func<T, string> memberNameFunc)
        {
            return Member(memberNameFunc(_instance));
        }
        
        public EnumSpecBuilder<T> Member(string memberName)
        {
            SetActiveMember(memberName);
            return this;
        }
    }
}