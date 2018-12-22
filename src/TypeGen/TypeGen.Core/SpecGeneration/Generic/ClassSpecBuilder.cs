using System;

namespace TypeGen.Core.SpecGeneration.Generic
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
    }
}