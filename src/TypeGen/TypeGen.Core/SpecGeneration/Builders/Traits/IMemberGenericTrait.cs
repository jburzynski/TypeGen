using System;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface IMemberGenericTrait<TType, TSpecBuilder>
{
    /// <summary>
    /// Sets the currently configured member using a lambda (for shorter notation).
    /// </summary>
    /// <param name="memberNameFunc">The lambda mapping a type instance to member name.</param>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder Member(Func<TType, string> memberNameFunc);
}