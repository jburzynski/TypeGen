using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    /// <summary>
    /// Base class for interface spec builders
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDerived"></typeparam>
    public abstract class CommonClassSpecBuilder<T, TDerived> : ClassOrInterfaceSpecBuilder<T, TDerived> where TDerived : CommonClassSpecBuilder<T, TDerived>
    {
        internal CommonClassSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
        
        /// <summary>
        /// Marks selected member as static (equivalent of TsStaticAttribute)
        /// </summary>
        /// <returns></returns>
        public TDerived Static()
        {
            AddActiveMemberAttribute(new TsStaticAttribute());
            return this as TDerived;
        }
        
        /// <summary>
        /// Marks selected member as not static (equivalent of TsNotStaticAttribute)
        /// </summary>
        /// <returns></returns>
        public TDerived NotStatic()
        {
            AddActiveMemberAttribute(new TsNotStaticAttribute());
            return this as TDerived;
        }
    }
}