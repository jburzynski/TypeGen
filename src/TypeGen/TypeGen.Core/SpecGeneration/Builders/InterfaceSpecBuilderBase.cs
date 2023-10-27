using System.Collections.Generic;
using TypeGen.Core.SpecGeneration.Builders.Traits;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders
{
    /// <summary>
    /// Builds the interface configuration section inside generation spec
    /// </summary>
    public abstract class InterfaceSpecBuilderBase<TSelf> : SpecBuilderBase,
        ICustomBaseTrait<TSelf>,
        ICustomHeaderTrait<TSelf>,
        ICustomBodyTrait<TSelf>,
        IDefaultExportTrait<TSelf>,
        IDefaultTypeOutputTrait<TSelf>,
        IDefaultValueTrait<TSelf>,
        IIgnoreBaseTrait<TSelf>,
        IIgnoreTrait<TSelf>,
        IMemberNameTrait<TSelf>,
        IMemberTrait<TSelf>,
        INotNullTrait<TSelf>,
        INotReadonlyTrait<TSelf>,
        INotUndefinedTrait<TSelf>,
        INullTrait<TSelf>,
        IOptionalTrait<TSelf>,
        IReadonlyTrait<TSelf>,
        ITypeTrait<TSelf>,
        ITypeUnionsTrait<TSelf>,
        IUndefinedTrait<TSelf>
        where TSelf : SpecBuilderBase
    {
        private readonly CustomBaseTrait<TSelf> _customBaseTrait;
        private readonly CustomBodyTrait<TSelf> _customBodyTrait;
        private readonly CustomHeaderTrait<TSelf> _customHeaderTrait;
        private readonly DefaultExportTrait<TSelf> _defaultExportTrait;
        private readonly DefaultTypeOutputTrait<TSelf> _defaultTypeOutputTrait;
        private readonly DefaultValueTrait<TSelf> _defaultValueTrait;
        private readonly IgnoreBaseTrait<TSelf> _ignoreBaseTrait;
        private readonly IgnoreTrait<TSelf> _ignoreTrait;
        private readonly MemberNameTrait<TSelf> _memberNameTrait;
        internal readonly MemberTrait<TSelf> MemberTrait;
        private readonly NotNullTrait<TSelf> _notNullTrait;
        private readonly NotReadonlyTrait<TSelf> _notReadonlyTrait;
        private readonly NotUndefinedTrait<TSelf> _notUndefinedTrait;
        private readonly NullTrait<TSelf> _nullTrait;
        private readonly OptionalTrait<TSelf> _optionalTrait;
        private readonly ReadonlyTrait<TSelf> _readonlyTrait;
        private readonly TypeTrait<TSelf> _typeTrait;
        private readonly TypeUnionsTrait<TSelf> _typeUnionsTrait;
        private readonly UndefinedTrait<TSelf> _undefinedTrait;
        
        internal InterfaceSpecBuilderBase(TypeSpec typeSpec) : base(typeSpec)
        {
            MemberTrait = new MemberTrait<TSelf>(this as TSelf, typeSpec);
            _customBaseTrait = new CustomBaseTrait<TSelf>(this as TSelf, typeSpec);
            _customBodyTrait = new CustomBodyTrait<TSelf>(this as TSelf, typeSpec);
            _customHeaderTrait = new CustomHeaderTrait<TSelf>(this as TSelf, typeSpec);
            _defaultExportTrait = new DefaultExportTrait<TSelf>(this as TSelf, typeSpec);
            _defaultTypeOutputTrait = new DefaultTypeOutputTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _defaultValueTrait = new DefaultValueTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _ignoreBaseTrait = new IgnoreBaseTrait<TSelf>(this as TSelf, typeSpec);
            _ignoreTrait = new IgnoreTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _memberNameTrait = new MemberNameTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _notNullTrait = new NotNullTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _notReadonlyTrait = new NotReadonlyTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _notUndefinedTrait = new NotUndefinedTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _nullTrait = new NullTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _optionalTrait = new OptionalTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _readonlyTrait = new ReadonlyTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _typeTrait = new TypeTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _typeUnionsTrait = new TypeUnionsTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _undefinedTrait = new UndefinedTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
        }
        
        /// <inheritdoc />
        public TSelf CustomBase(string @base = null, string importPath = null, string originalTypeName = null, bool isDefaultExport = false,
            IEnumerable<ImplementedInterface> implementedInterfaces = null)
            => _customBaseTrait.CustomBase(@base, importPath, originalTypeName, isDefaultExport, implementedInterfaces);
        
        /// <inheritdoc />
        public TSelf CustomBase(string @base = null, string importPath = null, string originalTypeName = null, bool isDefaultExport = false,
            params ImplementedInterface[] implementedInterfaces)
            => _customBaseTrait.CustomBase(@base, importPath, originalTypeName, isDefaultExport, implementedInterfaces);

        /// <inheritdoc />
        public TSelf CustomBody(string body)
            => _customBodyTrait.CustomBody(body);

        /// <inheritdoc />
        public TSelf CustomHeader(string header)
            => _customHeaderTrait.CustomHeader(header);
        
        /// <inheritdoc />
        public TSelf DefaultExport(bool enabled = true) => _defaultExportTrait.DefaultExport(enabled);
        
        /// <inheritdoc />
        public TSelf DefaultTypeOutput(string outputDir) => _defaultTypeOutputTrait.DefaultTypeOutput(outputDir);
        
        /// <inheritdoc />
        public TSelf DefaultValue(string defaultValue) => _defaultValueTrait.DefaultValue(defaultValue);
        
        /// <inheritdoc />
        public TSelf IgnoreBase() => _ignoreBaseTrait.IgnoreBase();
        
        /// <inheritdoc />
        public TSelf Ignore() => _ignoreTrait.Ignore();
        
        /// <inheritdoc />
        public TSelf MemberName(string name) => _memberNameTrait.MemberName(name);
        
        /// <inheritdoc />
        public TSelf Member(string memberName) => MemberTrait.Member(memberName);

        /// <inheritdoc />
        public TSelf NotNull() => _notNullTrait.NotNull();
        
        /// <inheritdoc />
        public TSelf NotReadonly() => _notReadonlyTrait.NotReadonly();

        /// <inheritdoc />
        public TSelf NotUndefined() => _notUndefinedTrait.NotUndefined();

        /// <inheritdoc />
        public TSelf Null() => _nullTrait.Null();
        
        /// <inheritdoc />
        public TSelf Optional() => _optionalTrait.Optional();
        
        /// <inheritdoc />
        public TSelf Readonly() => _readonlyTrait.Readonly();

        /// <inheritdoc />
        public TSelf Type(string typeName, string importPath = null, string originalTypeName = null, bool isDefaultExport = false)
            => _typeTrait.Type(typeName, importPath, originalTypeName, isDefaultExport);

        /// <inheritdoc />
        public TSelf Type(TsType tsType) => _typeTrait.Type(tsType);
        
        /// <inheritdoc />
        public TSelf TypeUnions(IEnumerable<string> typeUnions) => _typeUnionsTrait.TypeUnions(typeUnions);

        /// <inheritdoc />
        public TSelf TypeUnions(params string[] typeUnions) => _typeUnionsTrait.TypeUnions(typeUnions);

        /// <inheritdoc />
        public TSelf Undefined() => _undefinedTrait.Undefined();
    }
}
