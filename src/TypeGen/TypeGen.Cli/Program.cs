using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TypeGen.Cli.DependencyInjection;
using TypeGen.Core.Logging;
using TypeGen.Core.Storage;

namespace TypeGen.Cli;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddInterfacesWithSingleImplementation();
        services.AddTransient<IFileSystem, FileSystem>();
        services.AddTransient<ILogger, ConsoleLogger>();

        var serviceProvider = services.BuildServiceProvider(true);
        return (int)await serviceProvider.GetService<IApplication>().Run(args);
    }
}