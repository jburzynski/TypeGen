using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    public class CommonInterfaceSpecBuilder<T, TDerived> : ClassOrInterfaceSpecBuilder<T, TDerived> where TDerived : CommonInterfaceSpecBuilder<T, TDerived>
    {
        internal CommonInterfaceSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
        
        public TDerived Optional()
        {
            AddActiveMemberAttribute(new TsOptionalAttribute());
            return this as TDerived;
        }
    }
}