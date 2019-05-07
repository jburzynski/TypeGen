using System;

namespace TypeGen.Core.SpecGeneration.Generic
{
    /// <summary>
    /// Builds the enum configuration section inside generation spec
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumSpecBuilder<T> : CommonEnumSpecBuilder<T, EnumSpecBuilder<T>>
    {
        internal EnumSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
    }
}