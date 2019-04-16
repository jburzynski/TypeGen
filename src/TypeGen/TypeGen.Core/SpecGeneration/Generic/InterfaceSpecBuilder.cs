using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Generic
{
    /// <summary>
    /// Builds the interface configuration section inside generation spec
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InterfaceSpecBuilder<T> : CommonInterfaceSpecBuilder<T, InterfaceSpecBuilder<T>>
    {
        internal InterfaceSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
    }
}