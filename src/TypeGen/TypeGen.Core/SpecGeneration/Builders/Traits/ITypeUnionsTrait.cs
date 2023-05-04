using System.Collections.Generic;

namespace TypeGen.Core.SpecGeneration.Builders.Traits;

internal interface ITypeUnionsTrait<TSpecBuilder>
{
    /// <summary>
    /// Specifies TypeScript type unions (excluding the main type) for a property or field (equivalent of TsTypeUnionsAttribute).
    /// </summary>
    /// <param name="typeUnions">The type unions.</param>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder TypeUnions(IEnumerable<string> typeUnions);
    
    /// <summary>
    /// Specifies TypeScript type unions (excluding the main type) for a property or field (equivalent of TsTypeUnionsAttribute).
    /// </summary>
    /// <param name="typeUnions">The type unions.</param>
    /// <returns>The current instance of <see cref="TSpecBuilder"/>.</returns>
    TSpecBuilder TypeUnions(params string[] typeUnions);
}