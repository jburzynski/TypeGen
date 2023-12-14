using System;
using FluentAssertions;
using TypeGen.Core.Conversion;
using Xunit;

namespace TypeGen.Core.Test.Conversion;

public class XmlDocToTsDocConverterTest
{
    [Fact]
    public void Convert_Should_ConvertSummary()
    {
        // arrange
        var n = Environment.NewLine;
        var expected = $"/**{n}" +
                       $" * A nice summary.{n}" +
                       $" * Even multiline.{n}" +
                       $" * More lines.{n}" +
                       $" */";
        
        var xmlDoc = $"{n}" +
                     $"<summary>{n}" +
                     $"A nice summary.{n}" +
                     $"Even multiline.{n}" +
                     $"More lines.{n}" +
                     $"</summary>{n}";
        
        // act
        var actual = XmlDocToTsDocConverter.Convert(xmlDoc);
        
        // assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void Convert_Should_ConvertExample()
    {
        // arrange
        var n = Environment.NewLine;
        var expected = $"/**{n}" +
                       $" * @example{n}" +
                       $" * An example.{n}" +
                       $" * Even multiline.{n}" +
                       $" * More lines.{n}" +
                       $" */";
        
        var xmlDoc = $"{n}" +
                     $"<example>{n}" +
                     $"An example.{n}" +
                     $"Even multiline.{n}" +
                     $"More lines.{n}" +
                     $"</example>{n}";
        
        // act
        var actual = XmlDocToTsDocConverter.Convert(xmlDoc);
        
        // assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void Convert_Should_ConvertRemarks()
    {
        // arrange
        var n = Environment.NewLine;
        var expected = $"/**{n}" +
                       $" * @remarks{n}" +
                       $" * Remarks.{n}" +
                       $" * Even multiline.{n}" +
                       $" * More lines.{n}" +
                       $" */";
        
        var xmlDoc = $"{n}" +
                     $"<remarks>{n}" +
                     $"Remarks.{n}" +
                     $"Even multiline.{n}" +
                     $"More lines.{n}" +
                     $"</remarks>{n}";
        
        // act
        var actual = XmlDocToTsDocConverter.Convert(xmlDoc);
        
        // assert
        actual.Should().Be(expected);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void Convert_Should_ConvertSee(string trailingSpaces)
    {
        // arrange
        var n = Environment.NewLine;
        var expected = $"/**{n}" +
                       $" * A summary @see {{@link crefBody}}.{n}" +
                       $" */";
        
        var xmlDoc = $"{n}" +
                     $"<summary>{n}" +
                     $"A summary <see cref=\"!:crefBody\"{trailingSpaces}/>.{n}" +
                     $"</summary>{n}";
        
        // act
        var actual = XmlDocToTsDocConverter.Convert(xmlDoc);
        
        // assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void Convert_Should_ConvertTypeParam()
    {
        // arrange
        var n = Environment.NewLine;
        var expected = $"/**{n}" +
                       $" * @typeParam T - A nice param.{n}" +
                       $" */";

        var xmlDoc = $"{n}" +
                     $"<typeparam name=\"T\">A nice param.</typeparam>{n}";
        
        // act
        var actual = XmlDocToTsDocConverter.Convert(xmlDoc);
        
        // assert
        actual.Should().Be(expected);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void Convert_Should_ConvertInheritDocWithoutCref(string trailingSpaces)
    {
        // arrange
        var n = Environment.NewLine;
        var expected = $"/**{n}" +
                       $" * {{@inheritDoc}}{n}" +
                       $" */";

        var xmlDoc = $"{n}" +
                     $"<inheritdoc{trailingSpaces}/>{n}";
        
        // act
        var actual = XmlDocToTsDocConverter.Convert(xmlDoc);
        
        // assert
        actual.Should().Be(expected);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void Convert_Should_ConvertInheritDocWithCref(string trailingSpaces)
    {
        // arrange
        var n = Environment.NewLine;
        var expected = $"/**{n}" +
                       $" * {{@inheritDoc My.Cref}}{n}" +
                       $" */";

        var xmlDoc = $"{n}" +
                     $"<inheritdoc cref=\"My.Cref\"{trailingSpaces}/>{n}";
        
        // act
        var actual = XmlDocToTsDocConverter.Convert(xmlDoc);
        
        // assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void Convert_Should_ConvertAllTags()
    {
        // arrange
        var n = Environment.NewLine;
        var expected = $"/**{n}" +
                       $" * This is a summary.{n}" +
                       $" * Even multiline with @see {{@link My.Cref}}.{n}" +
                       $" * {n}" +
                       $" * @example{n}" +
                       $" * This is a simple example.{n}" +
                       $" * {n}" +
                       $" * @example{n}" +
                       $" * This is a{n}" +
                       $" * multiline example.{n}" +
                       $" * {n}" +
                       $" * @remarks{n}" +
                       $" * Be careful.{n}" +
                       $" * With this.{n}" +
                       $" * {n}" +
                       $" * @typeParam T - A nice param.{n}" +
                       $" * @typeParam V - Another param.{n}" +
                       $" */";

        var xmlDoc = $"{n}" +
                     $"<summary>{n}This is a summary.{n}Even multiline with <see cref=\"T:My.Cref\"/>.{n}</summary>{n}" +
                     $"<example>This is a simple example.</example>{n}" +
                     $"<example>{n}This is a{n}multiline example.{n}</example>{n}" +
                     $"<remarks>Be careful.{n}With this.</remarks>{n}" +
                     $"<typeparam name=\"T\">A nice param.</typeparam>{n}" +
                     $"<typeparam name=\"V\">Another param.</typeparam>{n}";
        
        // act
        var actual = XmlDocToTsDocConverter.Convert(xmlDoc);
        
        // assert
        actual.Should().Be(expected);
    }
}