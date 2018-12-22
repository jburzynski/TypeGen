using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Generic
{
    public class InterfaceSpecBuilder<T> : CommonInterfaceSpecBuilder<T, InterfaceSpecBuilder<T>>
    {
        internal InterfaceSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
        
        public InterfaceSpecBuilder<T> Member(Func<T, string> memberNameFunc)
        {
            return Member(memberNameFunc(_instance));
        }
    }
}