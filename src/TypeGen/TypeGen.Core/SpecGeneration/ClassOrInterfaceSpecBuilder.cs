using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    public abstract class ClassOrInterfaceSpecBuilder<T, TDerived> : TypeSpecBuilder<T> where TDerived : class
    {
        internal ClassOrInterfaceSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
        
        public TDerived CustomBase(string @base = null, string importType = null, string originalTypeName = null)
        {
            AddTypeAttribute(new TsCustomBaseAttribute(@base, importType, originalTypeName));
            return this as TDerived;
        }
        
        public TDerived DefaultTypeOutput(string outputDir)
        {
            AddActiveMemberAttribute(new TsDefaultTypeOutputAttribute(outputDir));
            return this as TDerived;
        }
        
        public TDerived DefaultValue(string defaultValue)
        {
            AddActiveMemberAttribute(new TsDefaultValueAttribute(defaultValue));
            return this as TDerived;
        }
        
        public TDerived Ignore()
        {
            AddActiveMemberAttribute(new TsIgnoreAttribute());
            return this as TDerived;
        }
        
        public TDerived IgnoreBase()
        {
            AddTypeAttribute(new TsIgnoreBaseAttribute());
            return this as TDerived;
        }
        
        public TDerived MemberName(string name)
        {
            AddActiveMemberAttribute(new TsMemberNameAttribute(name));
            return this as TDerived;
        }
        
        public TDerived NotNull()
        {
            AddActiveMemberAttribute(new TsNotNullAttribute());
            return this as TDerived;
        }
        
        public TDerived NotUndefined()
        {
            AddActiveMemberAttribute(new TsNotUndefinedAttribute());
            return this as TDerived;
        }
        
        public TDerived Null()
        {
            AddActiveMemberAttribute(new TsNullAttribute());
            return this as TDerived;
        }
        
        public TDerived Type(string typeName, string importPath = null, string originalTypeName = null)
        {
            AddActiveMemberAttribute(new TsTypeAttribute(typeName, importPath, originalTypeName));
            return this as TDerived;
        }
        
        public TDerived Undefined()
        {
            AddActiveMemberAttribute(new TsUndefinedAttribute());
            return this as TDerived;
        }
    }
}