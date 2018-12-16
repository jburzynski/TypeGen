using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    public abstract class ClassOrInterfaceSpecBuilder<T> : TypeSpecBuilder<T>
    {
        internal ClassOrInterfaceSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
        
        public ClassOrInterfaceSpecBuilder<T> CustomBase(string @base, string importType = null, string originalTypeName = null)
        {
            AddTypeAttribute(new TsCustomBaseAttribute(@base, importType, originalTypeName));
            return this;
        }
        
        public ClassOrInterfaceSpecBuilder<T> DefaultTypeOutput(string outputDir)
        {
            AddActiveMemberAttribute(new TsDefaultTypeOutputAttribute(outputDir));
            return this;
        }
        
        public ClassOrInterfaceSpecBuilder<T> DefaultValue(string defaultValue)
        {
            AddActiveMemberAttribute(new TsDefaultValueAttribute(defaultValue));
            return this;
        }
        
        public ClassOrInterfaceSpecBuilder<T> Ignore()
        {
            AddActiveMemberAttribute(new TsIgnoreAttribute());
            return this;
        }
        
        public ClassOrInterfaceSpecBuilder<T> IgnoreBase()
        {
            AddTypeAttribute(new TsIgnoreBaseAttribute());
            return this;
        }
        
        public ClassOrInterfaceSpecBuilder<T> MemberName(string name)
        {
            AddActiveMemberAttribute(new TsMemberNameAttribute(name));
            return this;
        }
        
        public ClassOrInterfaceSpecBuilder<T> NotNull()
        {
            AddActiveMemberAttribute(new TsNotNullAttribute());
            return this;
        }
        
        public ClassOrInterfaceSpecBuilder<T> NotUndefined()
        {
            AddActiveMemberAttribute(new TsNotUndefinedAttribute());
            return this;
        }
        
        public ClassOrInterfaceSpecBuilder<T> Null()
        {
            AddActiveMemberAttribute(new TsNullAttribute());
            return this;
        }
        
        public ClassOrInterfaceSpecBuilder<T> Type(string typeName, string importPath = null, string originalTypeName = null)
        {
            AddActiveMemberAttribute(new TsTypeAttribute(typeName, importPath, originalTypeName));
            return this;
        }
        
        public ClassOrInterfaceSpecBuilder<T> Undefined()
        {
            AddActiveMemberAttribute(new TsUndefinedAttribute());
            return this;
        }
    }
}