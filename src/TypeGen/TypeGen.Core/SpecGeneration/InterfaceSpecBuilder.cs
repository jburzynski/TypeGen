using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    public class InterfaceSpecBuilder<T> : ClassOrInterfaceSpecBuilder<T, InterfaceSpecBuilder<T>>
    {
        internal InterfaceSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
        
        public InterfaceSpecBuilder<T> Member(Func<T, string> memberNameFunc)
        {
            return Member(memberNameFunc(_instance));
        }
        
        public InterfaceSpecBuilder<T> Member(string memberName)
        {
            SetActiveMember(memberName);
            return this;
        }
        
        public InterfaceSpecBuilder<T> Optional()
        {
            AddActiveMemberAttribute(new TsOptionalAttribute());
            return this;
        }
    }
}