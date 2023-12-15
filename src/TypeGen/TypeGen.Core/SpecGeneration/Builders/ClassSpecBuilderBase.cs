using System.Collections.Generic;
using TypeGen.Core.SpecGeneration.Builders.Traits;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Builders
{
    /// <inheritdoc />
    public abstract class ClassSpecBuilderBase<TSelf> : SpecBuilderBase,
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
        INotStaticTrait<TSelf>,
        INotUndefinedTrait<TSelf>,
        INullTrait<TSelf>,
        IReadonlyTrait<TSelf>,
        IStaticTrait<TSelf>,
        ITypeTrait<TSelf>,
        ITypeUnionsTrait<TSelf>,
        IUndefinedTrait<TSelf>,
        IAdditionalPropertiesTrait<TSelf>
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
        private readonly NotStaticTrait<TSelf> _notStaticTrait;
        private readonly NotUndefinedTrait<TSelf> _notUndefinedTrait;
        private readonly NullTrait<TSelf> _nullTrait;
        private readonly ReadonlyTrait<TSelf> _readonlyTrait;
        private readonly StaticTrait<TSelf> _staticTrait;
        private readonly TypeTrait<TSelf> _typeTrait;
        private readonly TypeUnionsTrait<TSelf> _typeUnionsTrait;
        private readonly UndefinedTrait<TSelf> _undefinedTrait;
        private readonly AdditionalPropertiesTrait<TSelf> _additionalPropertiesTrait;

        internal ClassSpecBuilderBase(TypeSpec typeSpec) : base(typeSpec)
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
            _notStaticTrait = new NotStaticTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _notUndefinedTrait = new NotUndefinedTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _nullTrait = new NullTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _readonlyTrait = new ReadonlyTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _staticTrait = new StaticTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _typeTrait = new TypeTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _typeUnionsTrait = new TypeUnionsTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _undefinedTrait = new UndefinedTrait<TSelf>(this as TSelf, typeSpec, MemberTrait);
            _additionalPropertiesTrait = new AdditionalPropertiesTrait<TSelf>(this as TSelf, typeSpec);
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

        /// <summary>
        /// Adds an additional property to the TypeScript class being generated.
        /// The 'name' parameter can include access modifiers and the 'readonly' keyword,
        /// allowing for flexible property definitions. For instance, you can define the
        /// property as 'public name', 'private name', 'readonly name', etc.
        /// </summary>
        /// <param name="name">The name of the property, which can include access modifiers and keywords.</param>
        /// <param name="type">The TypeScript type of the property.</param>
        /// <param name="defaultValue">An optional default value for the property. If provided, it initializes the property with this value.</param>
        /// <returns>The instance of the spec builder for method chaining.</returns>
        public TSelf WithAdditionalProperty(string name, string type, string defaultValue = null) =>
           _additionalPropertiesTrait.WithAdditionalProperty(name, type, defaultValue);
    }
}