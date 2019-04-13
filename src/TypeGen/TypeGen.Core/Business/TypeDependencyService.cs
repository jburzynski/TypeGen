using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.Extensions;
using TypeGen.Core.TypeAnnotations;
using TypeGen.Core.Utils;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Business
{
    /// <summary>
    /// Retrieves information about type dependencies (i.e. types that a type depends on)
    /// </summary>
    internal class TypeDependencyService : ITypeDependencyService, IMetadataReaderSetter
    {
        private readonly ITypeService _typeService;
        private IMetadataReader _metadataReader;

        public TypeDependencyService(ITypeService typeService, IMetadataReader metadataReader)
        {
            _typeService = typeService;
            _metadataReader = metadataReader;
        }
        
        public void SetMetadataReader(IMetadataReader metadataReader)
        {
            _metadataReader = metadataReader;
        }
        
        /// <summary>
        /// Gets all non-simple and non-collection types the given type depends on.
        /// Types of properties/fields marked with TsIgnoreAttribute will be omitted.
        /// Returns an empty array if no dependencies were detected.
        /// Returns a distinct result (i.e. no duplicate TypeDependencyInfo instances)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the type is null</exception>
        public IEnumerable<TypeDependencyInfo> GetTypeDependencies(Type type)
        {
            Requires.NotNull(type, nameof(type));
            
            if (!type.GetTypeInfo().IsClass) return Enumerable.Empty<TypeDependencyInfo>();

            type = _typeService.StripNullable(type);

            return GetGenericTypeDefinitionDependencies(type)
                .Concat(GetBaseTypeDependency(type)
                .Concat(GetMemberTypeDependencies(type)))
                .Distinct(new TypeDependencyInfoTypeComparer<TypeDependencyInfo>());
        }

        /// <summary>
        /// Gets type dependencies related to generic type definition
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerable<TypeDependencyInfo> GetGenericTypeDefinitionDependencies(Type type)
        {
            IEnumerable<TypeDependencyInfo> result = Enumerable.Empty<TypeDependencyInfo>();

            if (!type.GetTypeInfo().IsGenericTypeDefinition) return result;

            foreach (Type genericArgumentType in type.GetGenericArguments())
            {
                Type baseType = genericArgumentType.GetTypeInfo().BaseType;
                if (baseType == null || baseType == typeof(object)) continue;

                baseType = _typeService.StripNullable(baseType);
                Type baseFlatType = _typeService.GetFlatType(baseType);

                result = result.Concat(GetFlatTypeDependencies(baseFlatType));
            }

            return result;
        }

        /// <summary>
        /// Gets the base type dependency for a type, if the base type exists
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerable<TypeDependencyInfo> GetBaseTypeDependency(Type type)
        {
            if (_metadataReader.GetAttribute<TsIgnoreBaseAttribute>(type) != null) return Enumerable.Empty<TypeDependencyInfo>();

            Type baseType = _typeService.GetBaseType(type);
            if (baseType == null) return Enumerable.Empty<TypeDependencyInfo>();

            return GetFlatTypeDependencies(baseType, null, true);
        }

        /// <summary>
        /// Gets type dependencies for the members inside a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerable<TypeDependencyInfo> GetMemberTypeDependencies(Type type)
        {
            IEnumerable<TypeDependencyInfo> result = Enumerable.Empty<TypeDependencyInfo>();

            IEnumerable<MemberInfo> memberInfos = _typeService.GetTsExportableMembers(type);
            foreach (MemberInfo memberInfo in memberInfos)
            {
                if (_metadataReader.GetAttribute<TsTypeAttribute>(memberInfo) != null) continue;

                Type memberType = _typeService.GetMemberType(memberInfo);
                Type memberFlatType = _typeService.GetFlatType(memberType);

                if (memberFlatType == type || (memberFlatType.IsConstructedGenericType && memberFlatType.GetGenericTypeDefinition() == type)) continue; // NOT a dependency if it's the type itself

                IEnumerable<Attribute> memberAttributes = _metadataReader.GetAttributes<Attribute>(memberInfo);
                result = result.Concat(GetFlatTypeDependencies(memberFlatType, memberAttributes));
            }

            return result;
        }

        private IEnumerable<TypeDependencyInfo> GetFlatTypeDependencies(Type flatType, IEnumerable<Attribute> memberAttributes = null, bool isBase = false)
        {
            if (_typeService.IsTsSimpleType(flatType) || flatType.IsGenericParameter) return Enumerable.Empty<TypeDependencyInfo>();
            
            if (flatType.GetTypeInfo().IsGenericType)
            {
                return GetGenericTypeNonDefinitionDependencies(flatType)
                    .Select(t => new TypeDependencyInfo(t, memberAttributes, isBase));
            }

            return new[] { new TypeDependencyInfo(flatType, memberAttributes, isBase) };
        }

        /// <summary>
        /// Gets type dependencies for a single generic member type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerable<Type> GetGenericTypeNonDefinitionDependencies(Type type)
        {
            if (!type.GetTypeInfo().IsGenericType) throw new CoreException($"Type {type.FullName} must be a generic type");
            
            IEnumerable<Type> result = _typeService.IsDictionaryType(type)
                ? new Type[0]
                : new[] { type.GetGenericTypeDefinition() };

            foreach (Type genericArgument in type.GetGenericArguments())
            {
                Type argumentType = _typeService.StripNullable(genericArgument);
                Type flatArgumentType = _typeService.GetFlatType(argumentType);
                if (_typeService.IsTsSimpleType(flatArgumentType) || flatArgumentType.IsGenericParameter) continue;

                result = result.Concat(flatArgumentType.GetTypeInfo().IsGenericType
                    ? GetGenericTypeNonDefinitionDependencies(flatArgumentType)
                    : new[] { flatArgumentType });
            }

            return result;
        }
    }
}
