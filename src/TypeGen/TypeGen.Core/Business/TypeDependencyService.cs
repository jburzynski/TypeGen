using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.Extensions;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.Business
{
    /// <summary>
    /// Contains logic for handling type dependencies (i.e. types that a type depends on)
    /// </summary>
    internal class TypeDependencyService
    {
        private readonly TypeService _typeService;

        public TypeDependencyService(TypeService typeService)
        {
            _typeService = typeService;
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

                baseType = _typeService.GetUnderlyingType(baseType);
                Type baseFlatType = _typeService.GetFlatType(baseType);
                if (_typeService.IsTsSimpleType(baseFlatType) || baseFlatType.IsGenericParameter) continue;

                if (baseFlatType.GetTypeInfo().IsGenericType)
                {
                    result = result.Concat(GetGenericTypeNonDefinitionDependencies(baseFlatType)
                        .Select(t => new TypeDependencyInfo(t)));
                }
                else
                {
                    result = result.Concat(new[] { new TypeDependencyInfo(baseFlatType) });
                }
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
            if (type.GetCustomAttribute<TsIgnoreBaseAttribute>() != null) yield break;

            Type baseType = _typeService.GetBaseType(type);
            if (baseType == null) yield break;

            if (baseType.GetTypeInfo().IsGenericType)
            {
                baseType = baseType.GetGenericTypeDefinition();
            }

            yield return new TypeDependencyInfo(baseType);
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
                if (memberInfo.GetCustomAttribute<TsTypeAttribute>() != null) continue;

                Type memberType = _typeService.GetMemberType(memberInfo);
                Type memberFlatType = _typeService.GetFlatType(memberType);

                if (memberFlatType == type || (memberFlatType.IsConstructedGenericType && memberFlatType.GetGenericTypeDefinition() == type)) continue; // NOT a dependency if it's the type itself
                if (_typeService.IsTsSimpleType(memberFlatType) || memberFlatType.IsGenericParameter) continue;

                var memberAttributes = memberInfo.GetCustomAttributes(typeof(Attribute), false) as Attribute[];

                if (memberFlatType.GetTypeInfo().IsGenericType)
                {
                    result = result.Concat(GetGenericTypeNonDefinitionDependencies(memberFlatType)
                        .Select(t => new TypeDependencyInfo(t, memberAttributes)));
                }
                else
                {
                    result = result.Concat(new[] { new TypeDependencyInfo(memberFlatType, memberAttributes) });
                }
            }

            return result;
        }

        /// <summary>
        /// Gets type dependencies for a single generic member type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerable<Type> GetGenericTypeNonDefinitionDependencies(Type type)
        {
            IEnumerable<Type> result = _typeService.IsDictionaryType(type)
                ? new Type[0]
                : new[] { type.GetGenericTypeDefinition() };

            foreach (Type genericArgument in type.GetGenericArguments())
            {
                Type argumentType = _typeService.GetUnderlyingType(genericArgument);
                Type flatArgumentType = _typeService.GetFlatType(argumentType);
                if (_typeService.IsTsSimpleType(flatArgumentType) || flatArgumentType.IsGenericParameter) continue;

                result = result.Concat(flatArgumentType.GetTypeInfo().IsGenericType
                    ? GetGenericTypeNonDefinitionDependencies(flatArgumentType)
                    : new[] { flatArgumentType });
            }

            return result;
        }

        /// <summary>
        /// Gets all non-simple and non-collection types the given type depends on.
        /// Types of properties/fields marked with TsIgnoreAttribute will be omitted.
        /// Returns an empty array if no dependencies were detected.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the type is null</exception>
        public IEnumerable<TypeDependencyInfo> GetTypeDependencies(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!type.GetTypeInfo().IsClass) return Enumerable.Empty<TypeDependencyInfo>();

            type = _typeService.GetUnderlyingType(type);

            return GetGenericTypeDefinitionDependencies(type)
                .Concat(GetBaseTypeDependency(type)
                .Concat(GetMemberTypeDependencies(type)))
                .Distinct(new TypeDependencyInfoTypeComparer<TypeDependencyInfo>());
        }
    }
}
