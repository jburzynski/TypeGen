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

    private const string GetForType_NestedTypeExists_XmlDocReturned_AssemblyName = "My.Assembly";
    private const string GetForType_NestedTypeExists_XmlDocReturned_TypeName = "My.Assembly.MyOuterType+MyNestedType";
    private const string GetForType_NestedTypeExists_XmlDocReturned_XmlDocForType = "<summary>My nested type.</summary>";
    private const string GetForType_NestedTypeExists_XmlDocReturned_XmlDoc = $"""
                                                                             <?xml version="1.0"?>
                                                                             <doc>
                                                                                 <assembly>
                                                                                 <name>{GetForType_NestedTypeExists_XmlDocReturned_AssemblyName}</name>
                                                                                 </assembly>
                                                                                 <members>
                                                                                 <member name="T:My.Assembly.MyOuterType.MyNestedType">
                                                                                     {GetForType_NestedTypeExists_XmlDocReturned_XmlDocForType}
                                                                                 </member>
                                                                             </members>
                                                                             </doc>
                                                                             """;
    
    [Fact]
    public void GetForType_NestedTypeExists_XmlDocReturned()
    {
        // arrange
        const string assemblyLocation = $"""C:\very\complicated\path\My.Assembly.dll""";
        const string xmlDocLocation = $"""C:\very\complicated\path\My.Assembly.xml""";
        
        var fileSystem = Substitute.For<IFileSystem>();
        fileSystem.FileExists(xmlDocLocation).Returns(true);
        fileSystem.ReadFile(xmlDocLocation).Returns(GetForType_NestedTypeExists_XmlDocReturned_XmlDoc);
        
        var xmlDocs = new XmlDocs(fileSystem);
        xmlDocs.Add(GetForType_NestedTypeExists_XmlDocReturned_AssemblyName, assemblyLocation);
        
        // act
        var actual = xmlDocs.GetForType(GetForType_NestedTypeExists_XmlDocReturned_AssemblyName,
            GetForType_NestedTypeExists_XmlDocReturned_TypeName);
        
        // assert
        actual.Should().Be(GetForType_NestedTypeExists_XmlDocReturned_XmlDocForType);
    }

    private const string GetForProperty_NestedTypePropertyExists_XmlDocReturned_AssemblyName = "My.Assembly";
    private const string GetForProperty_NestedTypePropertyExists_XmlDocReturned_TypeName = "My.Assembly.MyOuterType+MyNestedType";
    private const string GetForProperty_NestedTypePropertyExists_XmlDocReturned_PropertyName = "MyProperty";
    private const string GetForProperty_NestedTypePropertyExists_XmlDocReturned_XmlDocForProperty = "<summary>My nested type's property.</summary>";
    private const string GetForProperty_NestedTypePropertyExists_XmlDocReturned_XmlDoc = $"""
                                                                                         <?xml version="1.0"?>
                                                                                         <doc>
                                                                                             <assembly>
                                                                                             <name>{GetForProperty_NestedTypePropertyExists_XmlDocReturned_AssemblyName}</name>
                                                                                             </assembly>
                                                                                             <members>
                                                                                             <member name="P:My.Assembly.MyOuterType.MyNestedType.{GetForProperty_NestedTypePropertyExists_XmlDocReturned_PropertyName}">
                                                                                                 {GetForProperty_NestedTypePropertyExists_XmlDocReturned_XmlDocForProperty}
                                                                                             </member>
                                                                                         </members>
                                                                                         </doc>
                                                                                         """;
    
    [Fact]
    public void GetForProperty_NestedTypePropertyExists_XmlDocReturned()
    {
        // arrange
        const string assemblyLocation = $"""C:\very\complicated\path\My.Assembly.dll""";
        const string xmlDocLocation = $"""C:\very\complicated\path\My.Assembly.xml""";
        
        var fileSystem = Substitute.For<IFileSystem>();
        fileSystem.FileExists(xmlDocLocation).Returns(true);
        fileSystem.ReadFile(xmlDocLocation).Returns(GetForProperty_NestedTypePropertyExists_XmlDocReturned_XmlDoc);
        
        var xmlDocs = new XmlDocs(fileSystem);
        xmlDocs.Add(GetForProperty_NestedTypePropertyExists_XmlDocReturned_AssemblyName, assemblyLocation);
        
        // act
        var actual = xmlDocs.GetForProperty(GetForProperty_NestedTypePropertyExists_XmlDocReturned_AssemblyName,
            GetForProperty_NestedTypePropertyExists_XmlDocReturned_TypeName,
            GetForProperty_NestedTypePropertyExists_XmlDocReturned_PropertyName);
        
        // assert
        actual.Should().Be(GetForProperty_NestedTypePropertyExists_XmlDocReturned_XmlDocForProperty);
    }

    private const string GetForType_NestedTypeAndSimilarlyNamedTopLevelTypeExist_NestedTypeXmlDocReturned_AssemblyName = "My.Assembly";
    private const string GetForType_NestedTypeAndSimilarlyNamedTopLevelTypeExist_NestedTypeXmlDocReturned_TypeName = "My.Assembly.MyOuterType+MyNestedType";
    private const string GetForType_NestedTypeAndSimilarlyNamedTopLevelTypeExist_NestedTypeXmlDocReturned_XmlDocForType = "<summary>My nested type.</summary>";
    private const string GetForType_NestedTypeAndSimilarlyNamedTopLevelTypeExist_NestedTypeXmlDocReturned_XmlDoc = $"""
                                                                                                                   <?xml version="1.0"?>
                                                                                                                   <doc>
                                                                                                                       <assembly>
                                                                                                                       <name>{GetForType_NestedTypeAndSimilarlyNamedTopLevelTypeExist_NestedTypeXmlDocReturned_AssemblyName}</name>
                                                                                                                       </assembly>
                                                                                                                       <members>
                                                                                                                       <member name="T:My.Assembly.MyOuterTypeMyNestedType">
                                                                                                                           <summary>My top-level type.</summary>
                                                                                                                       </member>
                                                                                                                       <member name="T:My.Assembly.MyOuterType.MyNestedType">
                                                                                                                           {GetForType_NestedTypeAndSimilarlyNamedTopLevelTypeExist_NestedTypeXmlDocReturned_XmlDocForType}
                                                                                                                       </member>
                                                                                                                   </members>
                                                                                                                   </doc>
                                                                                                                   """;
    
    [Fact]
    public void GetForType_NestedTypeAndSimilarlyNamedTopLevelTypeExist_NestedTypeXmlDocReturned()
    {
        // arrange
        const string assemblyLocation = $"""C:\very\complicated\path\My.Assembly.dll""";
        const string xmlDocLocation = $"""C:\very\complicated\path\My.Assembly.xml""";
        
        var fileSystem = Substitute.For<IFileSystem>();
        fileSystem.FileExists(xmlDocLocation).Returns(true);
        fileSystem.ReadFile(xmlDocLocation).Returns(GetForType_NestedTypeAndSimilarlyNamedTopLevelTypeExist_NestedTypeXmlDocReturned_XmlDoc);
        
        var xmlDocs = new XmlDocs(fileSystem);
        xmlDocs.Add(GetForType_NestedTypeAndSimilarlyNamedTopLevelTypeExist_NestedTypeXmlDocReturned_AssemblyName, assemblyLocation);
        
        // act
        var actual = xmlDocs.GetForType(GetForType_NestedTypeAndSimilarlyNamedTopLevelTypeExist_NestedTypeXmlDocReturned_AssemblyName,
            GetForType_NestedTypeAndSimilarlyNamedTopLevelTypeExist_NestedTypeXmlDocReturned_TypeName);
        
        // assert
        actual.Should().Be(GetForType_NestedTypeAndSimilarlyNamedTopLevelTypeExist_NestedTypeXmlDocReturned_XmlDocForType);
    }
}