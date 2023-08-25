using System;
using FluentAssertions;
using TypeGen.Core.Generator;
using Xunit;

namespace TypeGen.Core.Test.Generator;

public class GeneratorOptionsTest
{
    [Theory]
    [InlineData(typeof(IConvertible), true)]
    [InlineData(typeof(GeneratorOptionsTest), false)]
    public void IsTypeBlacklisted_Test(Type type, bool expected)
    {
        var sut = new GeneratorOptions();
        sut.IsTypeBlacklisted(type).Should().Be(expected);
    }
    
    [Theory]
    [InlineData(typeof(IConvertible), false)]
    [InlineData(typeof(GeneratorOptionsTest), true)]
    public void IsTypeNotBlacklisted_Test(Type type, bool expected)
    {
        var sut = new GeneratorOptions();
        sut.IsTypeNotBlacklisted(type).Should().Be(expected);
    }
    
    [Fact]
    public void IsTypeBlacklisted_Should_MatchTypeFullName()
    {
        // arrange
        var sut = new GeneratorOptions();
        sut.TypeBlacklist.Add(GetType().FullName);
        
        // act
        // assert
        sut.IsTypeBlacklisted(GetType()).Should().BeTrue();
    }

    [Fact]
    public void IsTypeBlacklisted_Should_MatchTypeName()
    {
        // arrange
        var sut = new GeneratorOptions();
        sut.TypeBlacklist.Add(GetType().Name);
        
        // act
        // assert
        sut.IsTypeBlacklisted(GetType()).Should().BeTrue();
    }
}