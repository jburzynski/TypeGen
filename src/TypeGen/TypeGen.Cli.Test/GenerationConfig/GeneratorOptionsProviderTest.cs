using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NSubstitute;
using TypeGen.Cli.GenerationConfig;
using TypeGen.Cli.TypeResolution;
using TypeGen.Core.Generator;
using Xunit;

namespace TypeGen.Cli.Test.GenerationConfig;

public class GeneratorOptionsProviderTest
{
    [Fact]
    public void GetGeneratorOptions_Should_ConcatenateBlacklists()
    {
        // arrange
        const string projectFolder = "test";
        var tgConfigBlacklist = new[] { "MyType" };
        var tgConfig = GetDefaultTgConfigWithoutConverters();
        tgConfig.TypeBlacklist = tgConfigBlacklist;
        var expected = GeneratorOptions.DefaultTypeBlacklist.Concat(tgConfigBlacklist);

        var converterResolver = Substitute.For<IConverterResolver>();
        var sut = new GeneratorOptionsProvider(converterResolver);

        // act
        var actual = sut.GetGeneratorOptions(tgConfig, Enumerable.Empty<Assembly>(), projectFolder)
            .TypeBlacklist;

        // assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void GetGeneratorOptions_WhitelistGiven_WhitelistRemovesTypesFromBlacklist()
    {
        // arrange
        const string projectFolder = "test";
        var tgConfigBlacklist = new[] { "MyType" };
        var tgConfigWhitelist = new[] { typeof(IConvertible).FullName };
        var tgConfig = GetDefaultTgConfigWithoutConverters();
        tgConfig.TypeBlacklist = tgConfigBlacklist;
        tgConfig.TypeWhitelist = tgConfigWhitelist;
        var expected = GeneratorOptions.DefaultTypeBlacklist.Concat(tgConfigBlacklist).Except(tgConfigWhitelist);

        var converterResolver = Substitute.For<IConverterResolver>();
        var sut = new GeneratorOptionsProvider(converterResolver);

        // act
        var actual = sut.GetGeneratorOptions(tgConfig, Enumerable.Empty<Assembly>(), projectFolder)
            .TypeBlacklist;

        // assert
        actual.Should().BeEquivalentTo(expected);
    }

    private static TgConfig GetDefaultTgConfigWithoutConverters()
    {
        var tgConfig = new TgConfig();
        tgConfig.MergeWithDefaultParams();
        tgConfig.FileNameConverters = Array.Empty<string>();
        tgConfig.PropertyNameConverters = Array.Empty<string>();
        tgConfig.TypeNameConverters = Array.Empty<string>();
        tgConfig.EnumStringInitializersConverters = Array.Empty<string>();
        tgConfig.EnumValueNameConverters = Array.Empty<string>();

        return tgConfig;
    }
}