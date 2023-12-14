using System;
using System.Collections.Generic;
using System.Linq;
using TypeGen.Core.TypeAnnotations;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Extensions;

internal static class AttributeExtensions
{
    public static IEnumerable<Attribute> GetTypeGenAttributes(this IEnumerable<Attribute> @this)
    {
        Requires.NotNull(@this, nameof(@this));
        return @this.Where(x => x.IsTypeGenAttribute());
    }

    public static bool IsTypeGenAttribute(this Attribute @this)
    {
        Requires.NotNull(@this, nameof(@this));
        var tgAttributesNamespace = typeof(ExportAttribute).Namespace;
        return @this.GetType().Namespace == tgAttributesNamespace;
    }
}