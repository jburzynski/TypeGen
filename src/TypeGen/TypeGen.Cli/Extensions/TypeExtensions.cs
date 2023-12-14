using System;
using TypeGen.Core.Validation;

namespace TypeGen.Cli.Extensions;

internal static class TypeExtensions
{
    public static bool ImplementsInterface(this Type @this, string interfaceName)
    {
        Requires.NotNull(@this, nameof(@this));
        Requires.NotNullOrEmpty(interfaceName, nameof(interfaceName));

        return @this.GetInterface(interfaceName) != null;
    }
}