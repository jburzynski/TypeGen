using System;

namespace TypeGen.Core.SpecGeneration.Generic
{
    public class EnumSpecBuilder<T> : CommonEnumSpecBuilder<T, EnumSpecBuilder<T>>
    {
        internal EnumSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
        
        public EnumSpecBuilder<T> Member(Func<T, string> memberNameFunc)
        {
            return Member(memberNameFunc(_instance));
        }
    }
}