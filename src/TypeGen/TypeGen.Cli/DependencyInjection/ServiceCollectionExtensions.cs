using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TypeGen.Cli.Extensions;
using TypeGen.Core.Validation;

namespace TypeGen.Cli.DependencyInjection;

internal static class ServiceCollectionExtensions
{
    public static void AddInterfacesWithSingleImplementation(this ServiceCollection @this)
    {
        Requires.NotNull(@this, nameof(@this));
        
        var allTypes = Assembly.GetExecutingAssembly().GetTypes();

        var typePairs = allTypes.Where(@interface => @interface.IsInterface
                                 && allTypes.Count(@class => @class.IsClass && @class.ImplementsInterface(@interface.FullName)) == 1)
            .Select(@interface => (@interface, @class: allTypes.Single(@class => @class.ImplementsInterface(@interface.FullName))));

        foreach (var pair in typePairs)
            @this.AddTransient(pair.@interface, pair.@class);
    }
}