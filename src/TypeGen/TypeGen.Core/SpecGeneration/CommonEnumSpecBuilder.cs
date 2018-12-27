using TypeGen.Core.SpecGeneration.Generic;
using TypeGen.Core.TypeAnnotations;

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
        
        /// <summary>
        /// Specifies whether to use TypeScript string initializers for an enum
        /// </summary>
        /// <returns></returns>
        public TDerived StringInitializers(bool enabled = true)
        {
            AddTypeAttribute(new TsStringInitializersAttribute(enabled));
            return this as TDerived;
        }
    }
}