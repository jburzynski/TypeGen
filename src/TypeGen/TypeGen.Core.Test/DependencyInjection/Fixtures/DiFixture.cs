using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TypeGen.Core.Generator.Services;
using TypeGen.Core.Logging;
using TypeGen.Core.Metadata;
using TypeGen.Core.Storage;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace TypeGen.Core.Test.Fixtures;

public class DiFixture : TestBedFixture
{
    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<IMetadataReaderFactory, MetadataReaderFactory>();
        services.AddScoped<ITypeService, TypeService>();
        services.AddScoped<ITypeDependencyService, TypeDependencyService>();
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<ITsContentGenerator, TsContentGenerator>();
        services.AddScoped<ITsContentParser, TsContentParser>();
        services.AddScoped<IFileSystem, FileSystem>();
        services.AddScoped<IInternalStorage, InternalStorage>();
        services.AddScoped<Core.Generator.Generator>();
        services.AddScoped<ILogger, ConsoleLogger>();
        services.AddOptions();
    }

    protected override IEnumerable<TestAppSettings> GetTestAppSettings() => Array.Empty<TestAppSettings>();

    protected override ValueTask DisposeAsyncCore() => ValueTask.CompletedTask;
}