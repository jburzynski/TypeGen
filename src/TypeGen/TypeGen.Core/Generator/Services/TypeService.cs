using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using TypeGen.Core.Extensions;
using TypeGen.Core.Metadata;
using TypeGen.Core.TypeAnnotations;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Generator.Services
{
    /// <summary>
    /// Retrieves information about types
    /// </summary>
    internal class TypeService : ITypeService
    {
        private static HashSet<Type> IgnoredGenricConstraints = new()
        {
            typeof(ValueType)
        };

        private readonly IMetadataReaderFactory _metadataReaderFactory;
        private readonly IOptions<GeneratorOptions> _options;

        private GeneratorOptions GeneratorOptions => _options.Value;
        private IMetadataReader MetadataReader => _metadataReaderFactory.GetInstance();

        public TypeService(IMetadataReaderFactory metadataReaderFactory, IOptions<GeneratorOptions> options)
        {
            _metadataReaderFactory = metadataReaderFactory;
            _options = options;
        }

        /// <inheritdoc />
        public bool IsTsBuiltInType(Type type)
        {
            Requires.NotNull(type, nameof(type));
            return GetTsBuiltInTypeName(type) != null;
        }

        public bool IsEnumType(Type type)
        {
            Requires.NotNull(type, nameof(type));
            return type.IsEnum;
        }

        private string ConstructTsTypeName(Type type, string tsTypeNameTemplate)
        {
            // For custom types mappings ending with <>, construct the relevant generic custom type
            if (tsTypeNameTemplate.EndsWith("<>"))
            {
                tsTypeNameTemplate = tsTypeNameTemplate.Substring(0, tsTypeNameTemplate.Length - 2); // Strip <>
                string[] genericArgumentNames = type.GetGenericArguments()
                    .Select(t2 => t2.IsGenericParameter ? t2.Name : GetTsTypeName(t2, false))
                    .ToArray();
                tsTypeNameTemplate = $"{tsTypeNameTemplate}<{string.Join(", ", genericArgumentNames)}>";
            }

            // For custom types not ending with <>, leave the custom type as-is (not generic)
            return tsTypeNameTemplate;
        }

        private bool TryGetCustomTypeMapping(Type type, out string tsTypeName)
        {
            if (type is { FullName: not null } && GeneratorOptions.CustomTypeMappings != null)
            {
                // Check for given type as-is (and combined generics)
                if (GeneratorOptions.CustomTypeMappings.TryGetValue(type.FullName, out var customTypeMappingValue))
                {
                    tsTypeName = ConstructTsTypeName(type, customTypeMappingValue);
                    return true;
                }

                if (type.IsConstructedGenericType)
                {
                    // Check for generic type
                    Type genericType = type.GetGenericTypeDefinition();
                    if (GeneratorOptions.CustomTypeMappings.TryGetValue(genericType.FullName, out customTypeMappingValue))
                    {
                        tsTypeName = ConstructTsTypeName(type, customTypeMappingValue);
                        return true;
                    }
                }
            }
            tsTypeName = null;
            return false;
        }

        /// <inheritdoc />
        public string GetTsBuiltInTypeName(Type type)
        {
            Requires.NotNull(type, nameof(type));
            if (string.IsNullOrWhiteSpace(type.FullName)) return null;
            
            if (TryGetCustomTypeMapping(type, out string customType))
            {
                return new[] { "Object", "boolean", "string", "number", "Date" }.Contains(customType) ? customType : null;
            }

            switch (type.FullName)
            {
                case "System.Object":
                    return "Object";
                case "System.Boolean":
                    return "boolean";
                case "System.Char":
                case "System.String":
                case "System.Guid":
                    return "string";
                case "System.SByte":
                case "System.Byte":
                case "System.Int16":
                case "System.UInt16":
                case "System.Int32":
                case "System.UInt32":
                case "System.Int64":
                case "System.UInt64":
                case "System.Single":
                case "System.Double":
                case "System.Decimal":
                    return "number";
                case "System.DateTime":
                case "System.DateTimeOffset":
                    return "Date";
                default:
                    return null;
            }
        }

        /// <inheritdoc />
        public bool IsTsInterface(Type type)
        {
            Requires.NotNull(type, nameof(type));

            if (type.IsGenericType) type = type.GetGenericTypeDefinition();
            
            return MetadataReader.GetAttribute<ExportTsInterfaceAttribute>(type) != null
                || (!type.IsEnum && GeneratorOptions.ExportTypesAsInterfacesByDefault);
        }

        /// <inheritdoc />
        public Type GetMemberType(MemberInfo memberInfo)
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));

            if (!memberInfo.Is<FieldInfo>() && !memberInfo.Is<PropertyInfo>())
                throw new ArgumentException($"{memberInfo} must be either a FieldInfo or a PropertyInfo");

            return memberInfo is PropertyInfo info
                ? StripNullable(info.PropertyType)
                : StripNullable(((FieldInfo)memberInfo).FieldType);
        }

        /// <inheritdoc />
        public bool IsCollectionType(Type type)
        {
            Requires.NotNull(type, nameof(type));
            
            return type.FullName != "System.String" // not a string
                && !IsDictionaryType(type) // not a dictionary
                && (type.GetInterface("IEnumerable") != null || (type.FullName != null && type.FullName.StartsWith("System.Collections.IEnumerable"))); // implements IEnumerable or is IEnumerable
        }

        /// <inheritdoc />
        public bool IsDictionaryType(Type type)
        {
            Requires.NotNull(type, nameof(type));
            
            return type.GetInterface("System.Collections.Generic.IDictionary`2") != null
                   || (type.FullName != null && type.FullName.StartsWith("System.Collections.Generic.IDictionary`2"))
                   || type.GetInterface("System.Collections.Generic.IReadOnlyDictionary`2") != null
                   || (type.FullName != null && type.FullName.StartsWith("System.Collections.Generic.IReadOnlyDictionary`2"))
                   || type.GetInterface("System.Collections.IDictionary") != null
                   || (type.FullName != null && type.FullName.StartsWith("System.Collections.IDictionary"));
        }

        /// <inheritdoc />
        public bool IsCustomGenericType(Type type)
        {
            Requires.NotNull(type, nameof(type));
            return type.GetTypeInfo().IsGenericType && !IsDictionaryType(type) && !IsCollectionType(type);
        }

        /// <inheritdoc/>
        public bool IsIgnoredGenericConstarint(Type type)
        {
            return IgnoredGenricConstraints.Contains(type);
        }

        /// <inheritdoc />
        public bool UseDefaultExport(Type type)
        {
            Requires.NotNull(type, nameof(type));
            return MetadataReader.GetAttribute<TsDefaultExportAttribute>(type)?.Enabled ?? _options.Value.UseDefaultExport;
        }

        /// <inheritdoc />
        public string GetTsTypeName(Type type, bool forTypeDeclaration = false)
        {
            Requires.NotNull(type, nameof(type));
            Requires.NotNull(GeneratorOptions.TypeNameConverters, nameof(GeneratorOptions.TypeNameConverters));

            type = StripNullable(type);

            if (TryGetCustomTypeMapping(type, out string customType)) return customType;
            
            if (IsTsBuiltInType(type)) return GetTsBuiltInTypeName(type);
            if (IsCollectionType(type)) return GetTsCollectionTypeName(type);
            if (IsDictionaryType(type)) return GetTsDictionaryTypeName(type);
            if (IsCustomGenericType(type)) return GetGenericTsTypeName(type, forTypeDeclaration);

            string typeNameNoArity = type.Name.RemoveTypeArity();
            return type.IsGenericParameter ? typeNameNoArity : GeneratorOptions.TypeNameConverters.Convert(typeNameNoArity, type);
        }

        /// <inheritdoc />
        public string GetTsTypeName(MemberInfo memberInfo)
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));
            Requires.NotNull(GeneratorOptions.TypeNameConverters, nameof(GeneratorOptions.TypeNameConverters));
            
            var typeAttribute = MetadataReader.GetAttribute<TsTypeAttribute>(memberInfo);
            if (typeAttribute != null)
            {
                if (string.IsNullOrWhiteSpace(typeAttribute.TypeName))
                    throw new CoreException($"No type specified in TsType attribute for member '{memberInfo.Name}' declared in '{memberInfo.DeclaringType?.FullName}'");

                Type type = GetMemberType(memberInfo);
                return ConstructTsTypeName(type, typeAttribute.TypeName);
            }

            return GetTsTypeNameForMember(memberInfo);
        }

        public bool MemberTypeContainsBlacklistedType(MemberInfo memberInfo)
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));

            var type = GetMemberType(memberInfo);
            return TypeContainsBlacklistedType(type);
        }
        
        public bool TypeContainsBlacklistedType(Type type)
        {
            Requires.NotNull(type, nameof(type));

            if (type.IsGenericParameter) return false;
            
            type = StripNullable(type);
            if (GeneratorOptions.IsTypeBlacklisted(type)) return true;

            if (IsCollectionType(type))
            {
                var elementType = GetTsCollectionElementType(type);
                if (GeneratorOptions.IsTypeBlacklisted(elementType)) return true;
            }

            if (type.IsGenericType)
            {
                return type.GetGenericArguments()
                    .Select(TypeContainsBlacklistedType)
                    .Any(x => x);
            }

            return false;
        }

        /// <summary>
        /// Gets TypeScript type name for a member
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when member or typeNameConverters is null</exception>
        private string GetTsTypeNameForMember(MemberInfo memberInfo)
        {
            if (memberInfo.GetCustomAttribute<DynamicAttribute>() != null)
                return "any";

            Type type = GetMemberType(memberInfo);
            return GetTsTypeName(type, false);
        }

#if NET6_0_OR_GREATER
        private NullabilityInfoContext _nullabilityContext = new NullabilityInfoContext();
#endif

        public IEnumerable<string> GetTypeUnions(MemberInfo memberInfo)
        {
            const string nullLiteral = "null";
            const string undefinedLiteral = "undefined";
            
            Type memberType = memberInfo is PropertyInfo info
                ? info.PropertyType
                : ((FieldInfo)memberInfo).FieldType;
            
            var result = new List<string>();

            var tsTypeUnionsAttribute = MetadataReader.GetAttribute<TsTypeUnionsAttribute>(memberInfo);

            if (tsTypeUnionsAttribute != null)
            {
                // add from TsTypeUnionsAttribute
            
                result.AddRange(tsTypeUnionsAttribute.TypeUnions);
            }
            else
            {
                // add from both typeUnionsForTypes and csNullableTranslation
                
                string tsTypeName = GetTsTypeName(memberInfo);
                if (GeneratorOptions.TypeUnionsForTypes.ContainsKey(tsTypeName))
                {
                    result.AddRange(GeneratorOptions.TypeUnionsForTypes[tsTypeName]);
                }

                var nullable = memberInfo.IsNullable();

                if ((nullable && GeneratorOptions.CsNullableTranslation != StrictNullTypeUnionFlags.None && GeneratorOptions.CsNullableTranslation != StrictNullTypeUnionFlags.Optional) ||
                    GeneratorOptions.CsAllowNullsForAllTypes)
                {
                    if (GeneratorOptions.CsNullableTranslation.HasFlag(StrictNullTypeUnionFlags.Null)) result.Add(nullLiteral);
                    if (GeneratorOptions.CsNullableTranslation.HasFlag(StrictNullTypeUnionFlags.Undefined)) result.Add(undefinedLiteral);
                }

                result = result.Distinct().ToList();
            }
            
            // Ts[Null|NotNull|Undefined|NotUndefined]Attribute has the highest priority

            if (MetadataReader.GetAttribute<TsNullAttribute>(memberInfo) != null) result.Add(nullLiteral);
            if (MetadataReader.GetAttribute<TsUndefinedAttribute>(memberInfo) != null) result.Add(undefinedLiteral);

            if (MetadataReader.GetAttribute<TsNotNullAttribute>(memberInfo) != null) result.RemoveAll(x => x == nullLiteral);
            if (MetadataReader.GetAttribute<TsNotUndefinedAttribute>(memberInfo) != null) result.RemoveAll(x => x == undefinedLiteral);

            return result.Distinct().OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Gets TypeScript type name for a dictionary type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetTsDictionaryTypeName(Type type)
        {
            // handle IDictionary<,> and IReadOnlyDictionary<,>
            
            Type[] dictionary2Interfaces = new[] {
                type.GetInterface("System.Collections.Generic.IDictionary`2"),
                type.GetInterface("System.Collections.Generic.IReadOnlyDictionary`2")
            };
            Type dictionary2Interface = dictionary2Interfaces.LastOrDefault(i => i != null);
            if (dictionary2Interface != null || (type.FullName != null && (type.FullName.StartsWith("System.Collections.Generic.IDictionary`2") || type.FullName.StartsWith("System.Collections.Generic.IReadOnlyDictionary`2"))))
            {
                Type dictionaryType = dictionary2Interface ?? type;
                Type keyType = dictionaryType.GetGenericArguments()[0];
                Type valueType = dictionaryType.GetGenericArguments()[1];

                string keyTypeName = GetTsTypeName(keyType);
                string valueTypeName = GetTsTypeName(valueType);
                bool keyIsEnumType = IsEnumType(keyType);

                if (!keyTypeName.In("number", "string") && !keyIsEnumType)
                {
                    throw new CoreException($"Error when determining TypeScript type for C# type '{type.FullName}':" +
                                            " TypeScript dictionary key type must be either 'number', 'string' or an enum.");
                }

                return keyIsEnumType ? GetTsDictionaryTypeWithEnumKeyText(keyTypeName, valueTypeName) : GetTsDictionaryTypeText(keyTypeName, valueTypeName);
            }
            
            // handle IDictionary

            if (type.GetInterface("System.Collections.IDictionary") != null ||
                (type.FullName != null && type.FullName.StartsWith("System.Collections.IDictionary")))
            {
                return GetTsDictionaryTypeText("string", "string");
            }

            return null;
        }

        private string GetTsDictionaryTypeText(string keyTypeName, string valueTypeName) => $"{{ [key: {keyTypeName}]: {valueTypeName}; }}";
        private string GetTsDictionaryTypeWithEnumKeyText(string keyTypeName, string valueTypeName) => $"{{ [key in {keyTypeName}]?: {valueTypeName}; }}";

        /// <summary>
        /// Gets TypeScript type name for a collection type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetTsCollectionTypeName(Type type)
        {
            Type elementType = GetTsCollectionElementType(type);
            return GetTsTypeName(elementType) + "[]";
        }

        /// <summary>
        /// Gets TypeScript type name for a generic type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="forTypeDeclaration"></param>
        /// <returns></returns>
        private string GetGenericTsTypeName(Type type, bool forTypeDeclaration = false)
        {
            if (!forTypeDeclaration) return GetGenericTsTypeNameForNonDeclaration(type);

            return type.GetTypeInfo().IsGenericTypeDefinition
                ? GetGenericTsTypeNameForDeclaration(type)
                : GetGenericTsTypeNameForNonDeclaration(type);
        }

        /// <summary>
        /// Gets TypeScript type name for a generic type - used in type declarations
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetGenericTsTypeNameForDeclaration(Type type)
        {
            return GetGenericTsTypeNameDeclarationAgnostic(type, GetGenericTsTypeConstraintsForDeclaration);
        }

        /// <summary>
        /// Returns the string describing the generic parameter within a class or
        /// interface defenition. Generic type constraints will be added, matching the
        /// .net constraints as clolesly as possible
        /// </summary>
        /// <param name="type">
        /// Needs to be a genericParameter  (<see cref="Type.IsGenericParameter"/>
        /// flag set)
        /// </param>
        /// <returns></returns>
        private string GetGenericTsTypeConstraintsForDeclaration(Type type)
        {
            var constraints = type.GetGenericParameterConstraints().Where(t => !IsIgnoredGenericConstarint(t)).ToArray();
            if (constraints.Length < 1)
                return type.Name;

            var tsConstraints = constraints.Select(GetGenericTsTypeConstraintForDeclaration).Aggregate((a, b) => a + " & " + b);

            return $"{type.Name} extends { tsConstraints }";
        }

        /// <summary>
        /// Translates a .net type into the according ts constraint. <br/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetGenericTsTypeConstraintForDeclaration(Type type)
        {
            return GetTsTypeName(type, false);
        }

        /// <summary>
        /// Gets TypeScript type name for a generic type - used NOT in type declarations
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetGenericTsTypeNameForNonDeclaration(Type type)
        {
            return GetGenericTsTypeNameDeclarationAgnostic(type,
                t => t.IsGenericParameter ? t.Name : GetTsTypeName(t));
        }

        private string GetGenericTsTypeNameDeclarationAgnostic(Type type, Func<Type, string> genericArgumentsSelector)
        {
            string[] genericArgumentNames = type.GetGenericArguments()
                .Select(genericArgumentsSelector)
                .ToArray();

            string typeName = type.Name.RemoveTypeArity();
            string genericArgumentDef = string.Join(", ", genericArgumentNames);
            return $"{GeneratorOptions.TypeNameConverters.Convert(typeName, type)}<{genericArgumentDef}>";
        }

        /// <summary>
        /// Gets type of a collection element from the given type.
        /// If the passed type is not an array type or does not contain the IEnumerable interface, null is returned.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Type GetTsCollectionElementType(Type type)
        {
            Type elementType = type.GetElementType(); // this is for arrays
            if (elementType != null)
            {
                return StripNullable(elementType);
            }

            switch (type.Name)
            {
                case "IEnumerable`1":
                    return StripNullable(type.GetGenericArguments()[0]);
                case "IEnumerable":
                    return typeof(object);
            }

            // handle types implementing IEnumerable or IEnumerable<>

            Type ienumerable1Interface = type.GetInterface("IEnumerable`1");
            if (ienumerable1Interface != null) return StripNullable(ienumerable1Interface.GetGenericArguments()[0]);
            
            Type ienumerableInterface = type.GetInterface("IEnumerable");
            if (ienumerableInterface != null) return typeof(object);

            return null;
        }

        /// <inheritdoc />
        public Type GetFlatType(Type type)
        {
            Requires.NotNull(type, nameof(type));
            
            while (true)
            {
                if (!IsCollectionType(type)) return type;
                type = GetTsCollectionElementType(type);
            }
        }

        /// <inheritdoc />
        public Type StripNullable(Type type)
        {
            Requires.NotNull(type, nameof(type));
            
            Type nullableUnderlyingType = Nullable.GetUnderlyingType(type);
            return nullableUnderlyingType ?? type;
        }

        /// <inheritdoc />
        public Type GetBaseType(Type type)
        {
            Requires.NotNull(type, nameof(type));
            var baseType = type.GetTypeInfo().BaseType;
            
            if (IsNullOrObjectOrBlacklisted(baseType) || IsTsInterface(baseType))
                return null;

            return baseType;
        }

        /// <inheritdoc />
        public IEnumerable<Type> GetImplementedInterfaces(Type type)
        {
            Requires.NotNull(type, nameof(type));
            
            var result = type.GetTypeInfo().ImplementedInterfaces
                .Where(GeneratorOptions.IsTypeNotBlacklisted);

            var baseType = type.GetTypeInfo().BaseType;
            if (IsNotNullAndNotObjectAndNotBlacklisted(baseType) && IsTsInterface(baseType))
                result = result.Concat(new[] { baseType });

            return result;
        }

        private bool IsNullOrObjectOrBlacklisted(Type type) =>
            type == null
            || type == typeof(object)
            || GeneratorOptions.IsTypeBlacklisted(type);

        private bool IsNotNullAndNotObjectAndNotBlacklisted(Type type) =>
            !IsNullOrObjectOrBlacklisted(type);
    }
}
