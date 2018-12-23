using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    /// <summary>
    /// Base class for interface spec builders
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDerived"></typeparam>
    public class CommonInterfaceSpecBuilder<T, TDerived> : ClassOrInterfaceSpecBuilder<T, TDerived> where TDerived : CommonInterfaceSpecBuilder<T, TDerived>
    {
        internal CommonInterfaceSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
        
        /// <summary>
        /// Marks selected member as optional (equivalent of TsOptionalAttribute)
        /// </summary>
        /// <returns></returns>
        public TDerived Optional()
        {
            AddActiveMemberAttribute(new TsOptionalAttribute());
            return this as TDerived;
        }
    }
}