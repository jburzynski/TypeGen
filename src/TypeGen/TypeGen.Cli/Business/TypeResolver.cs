using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TypeGen.Core;
using TypeGen.Core.Extensions;
using TypeGen.Core.Generator;
using TypeGen.Core.Logging;
using TypeGen.Core.Storage;

namespace TypeGen.Cli.Business
{
    internal class TypeResolver
    {
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;
        
        private readonly string _projectFolder;
        private readonly IEnumerable<Assembly> _assemblies;

        private IEnumerable<Type> _interfaceConstraints;
        private IEnumerable<Type> _baseTypeConstraints;

        public TypeResolver(ILogger logger,
            IFileSystem fileSystem,
            string projectFolder,
            IEnumerable<Assembly> assemblies)
        {
            _logger = logger;
            _fileSystem = fileSystem;
            _projectFolder = projectFolder;
            _assemblies = assemblies;
        }

        public Type Resolve(string typeIdentifier, string typeNameSuffix = null, IEnumerable<Type> interfaceConstraints = null, IEnumerable<Type> baseTypeConstraints = null)
        {
            _interfaceConstraints = interfaceConstraints ?? Enumerable.Empty<Type>();
            _baseTypeConstraints = baseTypeConstraints ?? Enumerable.Empty<Type>();
            
            string[] nameParts = typeIdentifier.Split(':');

            if (nameParts.Length == 1)
            {
                return ResolveNoAssembly(typeIdentifier, typeNameSuffix);
            }

            if (nameParts.Length == 2)
            {
                return ResolveFromAssembly(nameParts[0], nameParts[1], _projectFolder);
            }

            throw new CliException($"Failed to load type '{typeIdentifier}'. Incorrect name format.");
        }
        
        private Type ResolveNoAssembly(string typeName, string typeNameSuffix)
        {
            Type result;

            // first, try to get the type from assemblies

            foreach (Assembly assembly in _assemblies)
            {
                result = ResolveFromAssembly(assembly, typeName, typeNameSuffix);
                if (result == null) continue;

                _logger.Log($"Type '{typeName}' found in assembly '{assembly.FullName}'", LogLevel.Debug);
                return result;
            }

            _logger.Log($"Type '{typeName}' not found in assemblies: '{string.Join(", ", _assemblies)}'. Falling back to TypeGen.Core.", LogLevel.Debug);

            // fallback to TypeGen.Core

            Assembly coreAssembly = typeof(Generator).Assembly;
            result = ResolveFromAssembly(coreAssembly, typeName, typeNameSuffix);
            if (result != null)
            {
                _logger.Log($"Type '{typeName}' found in TypeGen.Core", LogLevel.Debug);
                return result;
            }

            throw new CliException($"Type '{typeName}' not found in TypeGen.Core or any of the assemblies: '{string.Join("; ", _assemblies)}'");
        }

        private Type ResolveFromAssembly(string assemblyPath, string typeName, string typeNameSuffix)
        {
            string assemblyFullPath = Path.Combine(_projectFolder, assemblyPath);
            if (!_fileSystem.FileExists(assemblyFullPath))
            {
                throw new CliException($"Assembly path '{assemblyFullPath}' not found for type '{typeName}'");
            }

            Assembly converterAssembly = Assembly.LoadFrom(assemblyFullPath);
            return ResolveFromAssembly(converterAssembly, typeName, typeNameSuffix);
        }

        private Type ResolveFromAssembly(Assembly assembly, string typeName, string typeNameSuffix)
        {
            foreach (Type type in assembly.GetLoadableTypes())
            {
                bool nameMatches = (type.Name == typeName
                                   || type.Name == $"{typeName}{typeNameSuffix}"
                                   || type.FullName == typeName
                                   || type.FullName == $"{typeName}{typeNameSuffix}");

                var typeMatches = true;

                if (_interfaceConstraints.Any())
                {
                    typeMatches = _interfaceConstraints.Aggregate(typeMatches, (acc, intType) => acc && type.GetInterfaces().Any(i => i == intType));
                }

                if (_baseTypeConstraints.Any())
                {
                    typeMatches = _baseTypeConstraints.Aggregate(typeMatches, (acc, baseType) => acc && type.BaseType == baseType);
                }

                if (nameMatches && typeMatches)
                {
                    return type;
                }
            }

            return null;
        }
    }
}