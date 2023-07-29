using System;
using System.Collections.Generic;
using System.Reflection;
using TypeGen.Core.Conversion;
using TypeGen.Core.Storage;

namespace TypeGen.Core.Generator.Context
{
    /// <summary>
    /// Contains information relevant to the current source generation session.
    /// </summary>
    internal class GenerationContext
    {
        private readonly IFileSystem _fileSystem;

        private readonly GeneratedTypes _generatedTypes;
        private readonly XmlDocs _xmlDocs;

        public GenerationContext(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _xmlDocs = new XmlDocs(fileSystem);
            _generatedTypes = new GeneratedTypes();
        }

        public void BeginTypeGeneration(Type type)
        {
            _generatedTypes.BeginTypeGeneration();

            var assemblyName = type.Assembly.GetName().Name;
            var assemblyLocation = type.Assembly.Location;
            if (_xmlDocs.DoesNotContain(assemblyName)) _xmlDocs.Add(assemblyName, assemblyLocation);
        }

        public void EndTypeGeneration() => _generatedTypes.EndTypeGeneration();
        
        public void AddGeneratedType(Type type) => _generatedTypes.Add(type);
        
        public bool IsTypeGenerated(Type type) => _generatedTypes.IsGenerated(type);
        
        public bool IsTypeGeneratedForType(Type type) => _generatedTypes.IsGeneratedForType(type);

        public IEnumerable<Type> GetTypeGenerationStack() => _generatedTypes.GetTypeGenerationStack();

        public string GetXmlDocForMember(Type type, MemberInfo memberInfo)
        {
            var assemblyName = type.Assembly.GetName().Name;

            return memberInfo is PropertyInfo
                ? _xmlDocs.GetForProperty(assemblyName, type.FullName, memberInfo.Name)
                : _xmlDocs.GetForField(assemblyName, type.FullName, memberInfo.Name);
        }

        public string GetXmlDocForType(Type type)
        {
            var assemblyName = type.Assembly.GetName().Name;
            return _xmlDocs.GetForType(assemblyName, type.FullName);
        }

        public bool DoesNotContainXmlDocForAssembly(Assembly assembly)
        {
            var assemblyName = assembly.GetName().Name;
            return _xmlDocs.DoesNotContain(assemblyName);
        }
    }
}
