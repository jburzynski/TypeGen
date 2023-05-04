using System.Collections.Generic;
using TypeGen.Core.SpecGeneration.Builders.Traits;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders
{
    public abstract class ClassSpecBuilderBase<TSelf> : SpecBuilderBase,
        ICustomBaseTrait<TSelf>,
        IDefaultExportTrait<TSelf>,
        IDefaultTypeOutputTrait<TSelf>,
        IDefaultValueTrait<TSelf>,
        IIgnoreBaseTrait<TSelf>,
        IIgnoreTrait<TSelf>,
        IMemberNameTrait<TSelf>,
        IMemberTrait<TSelf>,
        INotNullTrait<TSelf>,
        INotReadonlyTrait<TSelf>,
        INotStaticTrait<TSelf>,
        INotUndefinedTrait<TSelf>,
        INullTrait<TSelf>,
        IReadonlyTrait<TSelf>,
        IStaticTrait<TSelf>,
        ITypeTrait<TSelf>,
        ITypeUnionsTrait<TSelf>,
        IUndefinedTrait<TSelf>
        where TSelf : SpecBuilderBase
    {
        private readonly CustomBaseTrait<TSelf> _customBaseTrait;
        private readonly DefaultExportTrait<TSelf> _defaultExportTrait;
        private readonly DefaultTypeOutputTrait<TSelf> _defaultTypeOutputTrait;
        private readonly DefaultValueTrait<TSelf> _defaultValueTrait;
        private readonly IgnoreBaseTrait<TSelf> _ignoreBaseTrait;
        private readonly IgnoreTrait<TSelf> _ignoreTrait;
        private readonly MemberNameTrait<TSelf> _memberNameTrait;
        internal readonly MemberTrait<TSelf> MemberTrait;
        private readonly NotNullTrait<TSelf> _notNullTrait;
        private readonly NotReadonlyTrait<TSelf> _notReadonlyTrait;
        private readonly NotStaticTrait<TSelf> _notStaticTrait;
        private readonly NotUndefinedTrait<TSelf> _notUndefinedTrait;
        private readonly NullTrait<TSelf> _nullTrait;
        private readonly ReadonlyTrait<TSelf> _readonlyTrait;
        private readonly StaticTrait<TSelf> _staticTrait;
        private readonly TypeTrait<TSelf> _typeTrait;
        private readonly TypeUnionsTrait<TSelf> _typeUnionsTrait;
        private readonly UndefinedTrait<TSelf> _undefinedTrait;
        
        internal ClassSpecBuilderBase(TypeSpec typeSpec) : base(typeSpec)
        {
            MemberTrait = new MemberTrait<TSelf>(this as TSelf, typeSpec);
            _customBaseTrait = new CustomBaseTrait<TSelf>(this as TSelf, typeSpec);
            _defaultExportTrait = new DefaultExportTrait<TSelf>(this as TSelf, typeSpec);
            _defaultTypeOutputTrait = new DefaultTypeOutputTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _defaultValueTrait = new DefaultValueTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _ignoreBaseTrait = new IgnoreBaseTrait<TSelf>(this as TSelf, typeSpec);
            _ignoreTrait = new IgnoreTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _memberNameTrait = new MemberNameTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _notNullTrait = new NotNullTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _notReadonlyTrait = new NotReadonlyTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _notStaticTrait = new NotStaticTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _notUndefinedTrait = new NotUndefinedTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _nullTrait = new NullTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _readonlyTrait = new ReadonlyTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _staticTrait = new StaticTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _typeTrait = new TypeTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _typeUnionsTrait = new TypeUnionsTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _undefinedTrait = new UndefinedTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
        }

        /// <inheritdoc />
        public TSelf CustomBase(string @base = null, string importPath = null, string originalTypeName = null, bool isDefaultExport = false)
            => _customBaseTrait.CustomBase(@base, importPath, originalTypeName, isDefaultExport);
        
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
        public TSelf NotStatic() => _notStaticTrait.NotStatic();

        /// <inheritdoc />
        public TSelf NotUndefined() => _notUndefinedTrait.NotUndefined();
        
        /// <inheritdoc />
        public TSelf Null() => _nullTrait.Null();
        
        /// <inheritdoc />
        public TSelf Readonly() => _readonlyTrait.Readonly();
        
        /// <inheritdoc />
        public TSelf Static() => _staticTrait.Static();

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