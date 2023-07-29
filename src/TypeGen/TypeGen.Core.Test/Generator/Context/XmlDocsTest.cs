using FluentAssertions;
using NSubstitute;
using TypeGen.Core.Generator.Context;
using TypeGen.Core.Storage;
using Xunit;

namespace TypeGen.Core.Test.Generator.Context;

public class XmlDocsTest
{
    private const string GetForProperty_PropertyExists_XmlDocReturned_AssemblyName = "My.Assembly";
    private const string GetForProperty_PropertyExists_XmlDocReturned_TypeName = "My.Assembly.MyType";
    private const string GetForProperty_PropertyExists_XmlDocReturned_PropertyName = "MyProperty";
    private const string GetForProperty_PropertyExists_XmlDocReturned_XmlDocForProperty = "<summary>My property.</summary>";
    private const string GetForProperty_PropertyExists_XmlDocReturned_XmlDoc = $"""
                                                                                <?xml version="1.0"?>
                                                                                        <doc>
                                                                                    <assembly>
                                                                                    <name>{GetForProperty_PropertyExists_XmlDocReturned_AssemblyName}</name>
                                                                                    </assembly>
                                                                                    <members>
                                                                                    <member name="P:{GetForProperty_PropertyExists_XmlDocReturned_TypeName}.{GetForProperty_PropertyExists_XmlDocReturned_PropertyName}">
                                                                                        {GetForProperty_PropertyExists_XmlDocReturned_XmlDocForProperty}
                                                                                    </member>
                                                                                </members>
                                                                                </doc>
                                                                                """;
    
    [Fact]
    public void GetForProperty_PropertyExists_XmlDocReturned()
    {
        // arrange
        const string assemblyLocation = $"""C:\very\complicated\path\My.Assembly.dll""";
        const string xmlDocLocation = $"""C:\very\complicated\path\My.Assembly.xml""";
        
        var fileSystem = Substitute.For<IFileSystem>();
        fileSystem.FileExists(xmlDocLocation).Returns(true);
        fileSystem.ReadFile(xmlDocLocation).Returns(GetForProperty_PropertyExists_XmlDocReturned_XmlDoc);
        
        var xmlDocs = new XmlDocs(fileSystem);
        xmlDocs.Add(GetForProperty_PropertyExists_XmlDocReturned_AssemblyName, assemblyLocation);
        
        // act
        var actual = xmlDocs.GetForProperty(GetForProperty_PropertyExists_XmlDocReturned_AssemblyName,
            GetForProperty_PropertyExists_XmlDocReturned_TypeName,
            GetForProperty_PropertyExists_XmlDocReturned_PropertyName);
        
        // assert
        actual.Should().Be(GetForProperty_PropertyExists_XmlDocReturned_XmlDocForProperty);
    }
}