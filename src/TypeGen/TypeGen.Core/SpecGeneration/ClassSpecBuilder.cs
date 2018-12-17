using System;

namespace TypeGen.Core.SpecGeneration
{
    public class ClassSpecBuilder<T> : ClassOrInterfaceSpecBuilder<T, ClassSpecBuilder<T>>
    {
        internal ClassSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
        
        public ClassSpecBuilder<T> Member(Func<T, string> memberNameFunc)
        {
            return Member(memberNameFunc(_instance));
        }
        
        public ClassSpecBuilder<T> Member(string memberName)
        {
            SetActiveMember(memberName);
            return this;
        }
    }
}