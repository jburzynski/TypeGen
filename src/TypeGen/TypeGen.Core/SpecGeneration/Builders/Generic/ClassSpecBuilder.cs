using System;
using TypeGen.Core.SpecGeneration.Builders.Traits;

namespace TypeGen.Core.SpecGeneration.Builders.Generic
{
    /// <summary>
    /// Builds the class configuration section inside generation spec
    /// </summary>
    public class ClassSpecBuilder<TType> : ClassSpecBuilderBase<ClassSpecBuilder<TType>>,
        IMemberGenericTrait<TType, ClassSpecBuilder<TType>>
    {
        private readonly MemberGenericTrait<TType, ClassSpecBuilder<TType>> _memberGenericTrait;

        internal ClassSpecBuilder(TypeSpec typeSpec) : base(typeSpec)
        {
            _memberGenericTrait = new MemberGenericTrait<TType, ClassSpecBuilder<TType>>(this, typeSpec, MemberTrait);
        }

        /// <inheritdoc />
        public ClassSpecBuilder<TType> Member(Func<TType, string> memberNameFunc) => _memberGenericTrait.Member(memberNameFunc);
    }
}