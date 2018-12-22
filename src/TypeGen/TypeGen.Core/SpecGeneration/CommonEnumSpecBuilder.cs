using TypeGen.Core.SpecGeneration.Generic;

namespace TypeGen.Core.SpecGeneration
{
    public class CommonEnumSpecBuilder<T, TDerived> : TypeSpecBuilder<T, TDerived> where TDerived : CommonEnumSpecBuilder<T, TDerived>
    {
        internal CommonEnumSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
    }
}