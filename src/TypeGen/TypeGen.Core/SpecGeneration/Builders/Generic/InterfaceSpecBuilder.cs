using System;
using TypeGen.Core.SpecGeneration.Builders.Traits;

namespace TypeGen.Core.SpecGeneration.Builders.Generic
{
    /// <summary>
    /// Builds the interface configuration section inside generation spec
    /// </summary>
    public class InterfaceSpecBuilder<TType> : InterfaceSpecBuilderBase<InterfaceSpecBuilder<TType>>,
        IMemberGenericTrait<TType, InterfaceSpecBuilder<TType>>
    {
        private readonly MemberGenericTrait<TType, InterfaceSpecBuilder<TType>> _memberGenericTrait;
        
        internal InterfaceSpecBuilder(TypeSpec typeSpec) : base(typeSpec)
        {
            _memberGenericTrait = new MemberGenericTrait<TType, InterfaceSpecBuilder<TType>>(this, typeSpec, MemberTrait);
        }
        
        /// <inheritdoc />
        public InterfaceSpecBuilder<TType> Member(Func<TType, string> memberNameFunc) => _memberGenericTrait.Member(memberNameFunc);
    }
}