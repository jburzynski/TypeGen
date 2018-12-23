using TypeGen.Core.SpecGeneration.Generic;

namespace TypeGen.Core.SpecGeneration
{
    /// <summary>
    /// Base class for enum spec builders
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDerived"></typeparam>
    public abstract class CommonEnumSpecBuilder<T, TDerived> : TypeSpecBuilder<T, TDerived> where TDerived : CommonEnumSpecBuilder<T, TDerived>
    {
        internal CommonEnumSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
    }
}