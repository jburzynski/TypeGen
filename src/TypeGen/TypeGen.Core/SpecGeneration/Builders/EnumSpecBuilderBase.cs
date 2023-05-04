using TypeGen.Core.SpecGeneration.Builders.Traits;

namespace TypeGen.Core.SpecGeneration.Builders
{
    /// <summary>
    /// Builds the enum configuration section inside generation spec
    /// </summary>
    public abstract class EnumSpecBuilderBase<TSelf> : SpecBuilderBase,
        IDefaultExportTrait<TSelf>,
        IMemberTrait<TSelf>,
        IStringInitializersTrait<TSelf>
        where TSelf: SpecBuilderBase
    {
        private readonly DefaultExportTrait<TSelf> _defaultExportTrait;
        internal readonly MemberTrait<TSelf> MemberTrait;
        private readonly StringInitializersTrait<TSelf> _stringInitializersTrait;
        
        internal EnumSpecBuilderBase(TypeSpec typeSpec) : base(typeSpec)
        {
            MemberTrait = new MemberTrait<TSelf>(this as TSelf, typeSpec);
            _defaultExportTrait = new DefaultExportTrait<TSelf>(this as TSelf, typeSpec);
            _stringInitializersTrait = new StringInitializersTrait<TSelf>(this as TSelf, typeSpec);
        }

        /// <inheritdoc />
        public TSelf DefaultExport(bool enabled = true) => _defaultExportTrait.DefaultExport(enabled);

        /// <inheritdoc />
        public TSelf Member(string memberName) => MemberTrait.Member(memberName);

        /// <inheritdoc />
        public TSelf StringInitializers(bool enabled = true) => _stringInitializersTrait.StringInitializers(enabled);
    }
}