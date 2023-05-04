using System;
using TypeGen.Core.SpecGeneration.Builders.Traits;

namespace TypeGen.Core.SpecGeneration.Builders.Generic
{
    /// <summary>
    /// Builds the enum configuration section inside generation spec
    /// </summary>
    public class EnumSpecBuilder<TType> : EnumSpecBuilderBase<EnumSpecBuilder<TType>>,
        IMemberGenericTrait<TType, EnumSpecBuilder<TType>>
    {
        private readonly MemberGenericTrait<TType, EnumSpecBuilder<TType>> _memberGenericTrait;
        
        internal EnumSpecBuilder(TypeSpec typeSpec) : base(typeSpec)
        {
            _memberGenericTrait = new MemberGenericTrait<TType, EnumSpecBuilder<TType>>(this, typeSpec, MemberTrait);
        }
        
        /// <inheritdoc />
        public EnumSpecBuilder<TType> Member(Func<TType, string> memberNameFunc) => _memberGenericTrait.Member(memberNameFunc);
    }
}