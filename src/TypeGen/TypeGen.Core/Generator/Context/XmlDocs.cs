using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TypeGen.Core.Conversion;
using TypeGen.Core.Storage;

namespace TypeGen.Core.Generator.Context;

/// <summary>
/// Contains the xml docs for assemblies.
/// </summary>
internal class XmlDocs
{
    private readonly IFileSystem _fileSystem;
    private readonly IDictionary<string, string> _assemblyToXmlDocMapping;

    public XmlDocs(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
        _assemblyToXmlDocMapping = new Dictionary<string, string>();
    }

    public void Add(string assemblyName, string assemblyLocation)
    {
        if (_assemblyToXmlDocMapping.ContainsKey(assemblyName))
            throw new InvalidOperationException($"Xml doc for assembly '{assemblyName}' has already been added.");

        var xmlDocFilePath = Path.ChangeExtension(assemblyLocation, "xml");
        
        var xmlDoc = !String.IsNullOrEmpty(assemblyLocation) && _fileSystem.FileExists(xmlDocFilePath)
            ? _fileSystem.ReadFile(xmlDocFilePath)
            : null;

        _assemblyToXmlDocMapping[assemblyName] = xmlDoc;
    }

    public string GetForProperty(string assemblyName, string typeFullName, string propertyName)
        => GetForXmlDocMember(assemblyName, XmlDocMemberType.Property, typeFullName, propertyName);
    
    public string GetForField(string assemblyName, string typeFullName, string fieldName)
        => GetForXmlDocMember(assemblyName, XmlDocMemberType.Field, typeFullName, fieldName);
    
    public string GetForType(string assemblyName, string typeFullName)
        => GetForXmlDocMember(assemblyName, XmlDocMemberType.Type, typeFullName);

    private string GetForXmlDocMember(string assemblyName, string xmlDocMemberType, string typeFullName, string memberName = null)
    {
        if (!_assemblyToXmlDocMapping.ContainsKey(assemblyName))
            throw new InvalidOperationException($"Assembly '{assemblyName}' has not been previously added.");
        
        var xmlDoc = _assemblyToXmlDocMapping[assemblyName];
        if (xmlDoc == null) return null;
        
        var typeFullNameForRegex = typeFullName.Replace(".", "\\.");
        var memberNameForRegex = memberName != null ? "\\." + memberName : null;
        
        var regex = new Regex($"""<member name="{xmlDocMemberType}:{typeFullNameForRegex}{memberNameForRegex}\">(.*?)<\/member>""",
            RegexOptions.Singleline);
        var match = regex.Match(xmlDoc);
        
        return match.Success ? match.Groups[1].Value.Trim() : null;
    }

    public bool Contains(string assemblyName) => _assemblyToXmlDocMapping.ContainsKey(assemblyName);
    
    public bool DoesNotContain(string assemblyName) => !Contains(assemblyName);
}