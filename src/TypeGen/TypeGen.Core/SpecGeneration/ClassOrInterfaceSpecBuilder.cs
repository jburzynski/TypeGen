using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    /// <summary>
    /// Base class for class and interface spec builders
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDerived"></typeparam>
    public abstract class ClassOrInterfaceSpecBuilder<T, TDerived> : TypeSpecBuilder<T, TDerived> where TDerived : ClassOrInterfaceSpecBuilder<T, TDerived>
    {
        internal ClassOrInterfaceSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
        
        /// <summary>
        /// Specifies custom base for the type (equivalent of TsCustomBaseAttribute)
        /// </summary>
        /// <param name="base"></param>
        /// <param name="importPath"></param>
        /// <param name="originalTypeName"></param>
        /// <returns></returns>
        public TDerived CustomBase(string @base = null, string importPath = null, string originalTypeName = null)
        {
            AddTypeAttribute(new TsCustomBaseAttribute(@base, importPath, originalTypeName));
            return this as TDerived;
        }
        
        /// <summary>
        /// Specifies the default type output path for the selected member (equivalent of TsDefaultTypeOutputAttribute)
        /// </summary>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        public TDerived DefaultTypeOutput(string outputDir)
        {
            AddActiveMemberAttribute(new TsDefaultTypeOutputAttribute(outputDir));
            return this as TDerived;
        }
        
        /// <summary>
        /// Specifies default value for the selected member (equivalent of TsDefaultValueAttribute)
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public TDerived DefaultValue(string defaultValue)
        {
            AddActiveMemberAttribute(new TsDefaultValueAttribute(defaultValue));
            return this as TDerived;
        }
        
        /// <summary>
        /// Marks selected member as ignored (equivalent of TsIgnoreAttribute)
        /// </summary>
        /// <returns></returns>
        public TDerived Ignore()
        {
            AddActiveMemberAttribute(new TsIgnoreAttribute());
            return this as TDerived;
        }
        
        /// <summary>
        /// Indicates whether to ignore base class declaration for type (equivalent of TsIgnoreBaseAttribute)
        /// </summary>
        /// <returns></returns>
        public TDerived IgnoreBase()
        {
            AddTypeAttribute(new TsIgnoreBaseAttribute());
            return this as TDerived;
        }
        
        /// <summary>
        /// Specifies name for the selected member (equivalent of TsMemberNameAttribute)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TDerived MemberName(string name)
        {
            AddActiveMemberAttribute(new TsMemberNameAttribute(name));
            return this as TDerived;
        }
        
        /// <summary>
        /// Marks selected member as not null (equivalent of TsNotNullAttribute)
        /// </summary>
        /// <returns></returns>
        public TDerived NotNull()
        {
            AddActiveMemberAttribute(new TsNotNullAttribute());
            return this as TDerived;
        }
        
        /// <summary>
        /// Marks selected member as not readonly (equivalent of TsNotReadonlyAttribute)
        /// </summary>
        /// <returns></returns>
        public TDerived NotReadonly()
        {
            AddActiveMemberAttribute(new TsNotReadonlyAttribute());
            return this as TDerived;
        }
        
        /// <summary>
        /// Marks selected member as not undefined (equivalent of TsNotUndefinedAttribute)
        /// </summary>
        /// <returns></returns>
        public TDerived NotUndefined()
        {
            AddActiveMemberAttribute(new TsNotUndefinedAttribute());
            return this as TDerived;
        }
        
        /// <summary>
        /// Marks selected member as null (equivalent of TsNullAttribute)
        /// </summary>
        /// <returns></returns>
        public TDerived Null()
        {
            AddActiveMemberAttribute(new TsNullAttribute());
            return this as TDerived;
        }
        
        /// <summary>
        /// Marks selected member as readonly (equivalent of TsReadonlyAttribute)
        /// </summary>
        /// <returns></returns>
        public TDerived Readonly()
        {
            AddActiveMemberAttribute(new TsReadonlyAttribute());
            return this as TDerived;
        }
        
        /// <summary>
        /// Specifies custom type for the selected member (equivalent of TsTypeAttribute)
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="importPath"></param>
        /// <param name="originalTypeName"></param>
        /// <returns></returns>
        public TDerived Type(string typeName, string importPath = null, string originalTypeName = null)
        {
            AddActiveMemberAttribute(new TsTypeAttribute(typeName, importPath, originalTypeName));
            return this as TDerived;
        }
        
        /// <summary>
        /// Specifies custom type for the selected member (equivalent of TsTypeAttribute)
        /// </summary>
        /// <returns></returns>
        public TDerived Type(TsType tsType)
        {
            AddActiveMemberAttribute(new TsTypeAttribute(tsType));
            return this as TDerived;
        }
        
        /// <summary>
        /// Marks selected member as undefined (equivalent of TsUndefinedAttribute)
        /// </summary>
        /// <returns></returns>
        public TDerived Undefined()
        {
            AddActiveMemberAttribute(new TsUndefinedAttribute());
            return this as TDerived;
        }
    }
}