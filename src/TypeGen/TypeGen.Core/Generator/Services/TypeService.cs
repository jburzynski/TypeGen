using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        private readonly IMetadataReaderFactory _metadataReaderFactory;
        private readonly IGeneratorOptionsProvider _generatorOptionsProvider;

        private GeneratorOptions GeneratorOptions => _generatorOptionsProvider.GeneratorOptions;
        private IMetadataReader MetadataReader => _metadataReaderFactory.GetInstance();

        public TypeService(IMetadataReaderFactory metadataReaderFactory, IGeneratorOptionsProvider generatorOptionsProvider)
        {
            _metadataReaderFactory = metadataReaderFactory;
            _generatorOptionsProvider = generatorOptionsProvider;
        }

        /// <inheritdoc />
        public bool IsTsSimpleType(Type type)
        {
            Requires.NotNull(type, nameof(type));

            return GetTsSimpleTypeName(type) != null;
        }

        private string GenerateCustomType(Type t, string customType)
        {
            // For custom types mappings ending with <>, construct the relevant generic custom type
            if (customType.EndsWith("<>"))
            {
                customType = customType.Substring(0, customType.Length - 2); // Strip <>
                string[] genericArgumentNames = t.GetGenericArguments()
                    .Select(t2 => t2.IsGenericParameter ? t2.Name : GetTsTypeName(t2, false))
                    .ToArray();
                customType = $"{customType}<{string.Join(", ", genericArgumentNames)}>";
            }

            // For custom types not ending with <>, leave the custom type as-is (not generic)
            return customType;
        }

        private bool TryGetCustomTypeMapping(Type t, out string customType)
        {
            if (t != null && t.FullName != null && GeneratorOptions.CustomTypeMappings != null)
            {
                // Check for given type as-is (and combined generics)
                if (GeneratorOptions.CustomTypeMappings.TryGetValue(t.FullName, out string customTypeMappingValue))
                {
                    customType = GenerateCustomType(t, customTypeMappingValue);
                    return true;
                }
                else if (t.IsConstructedGenericType)
                {
                    // Check for generic type
                    Type genericType = t.GetGenericTypeDefinition();
                    if (GeneratorOptions.CustomTypeMappings.TryGetValue(genericType.FullName, out customTypeMappingValue))
                    {
                        customType = GenerateCustomType(t, customTypeMappingValue);
                        return true;
                    }
                }
            }
            customType = null;
            return false;
        }

        /// <inheritdoc />
        public string GetTsSimpleTypeName(Type type)
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
        public bool IsTsClass(Type type)
        {
            Requires.NotNull(type, nameof(type));
            TypeInfo typeInfo = type.GetTypeInfo();

            if (!typeInfo.IsClass) return false;

            var exportAttribute = MetadataReader.GetAttribute<ExportAttribute>(type);
            return exportAttribute == null || exportAttribute is ExportTsClassAttribute;
        }

        /// <inheritdoc />
        public bool IsTsInterface(Type type)
        {
            Requires.NotNull(type, nameof(type));
            TypeInfo typeInfo = type.GetTypeInfo();

            if (!typeInfo.IsClass) return false;

            var exportAttribute = MetadataReader.GetAttribute<ExportAttribute>(type);
            return exportAttribute is ExportTsInterfaceAttribute;
        }

        /// <inheritdoc />
        public Type GetMemberType(MemberInfo memberInfo)
        {
            Requires.NotNull(memberInfo, nameof(memberInfo));

            if (!memberInfo.Is<FieldInfo>() && !memberInfo.Is<PropertyInfo>())
            {
                throw new ArgumentException($"{memberInfo} must be either a FieldInfo or a PropertyInfo");
            }

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
                   || type.GetInterface("System.Collections.IDictionary") != null
                   || (type.FullName != null && type.FullName.StartsWith("System.Collections.IDictionary"));
        }

        /// <inheritdoc />
        public bool IsCustomGenericType(Type type)
        {
            Requires.NotNull(type, nameof(type));
            return type.GetTypeInfo().IsGenericType && !IsDictionaryType(type) && !IsCollectionType(type);
        }

        /// <inheritdoc />
        public bool UseDefaultExport(Type type)
        {
            Requires.NotNull(type, nameof(type));
            return MetadataReader.GetAttribute<TsDefaultExportAttribute>(type)?.Enabled ?? _generatorOptionsProvider.GeneratorOptions.UseDefaultExport;
        }

        /// <inheritdoc />
        public string GetTsTypeName(Type type, bool forTypeDeclaration = false)
        {
            Requires.NotNull(type, nameof(type));
            Requires.NotNull(GeneratorOptions.TypeNameConverters, nameof(GeneratorOptions.TypeNameConverters));

            type = StripNullable(type);

            if (TryGetCustomTypeMapping(type, out string customType))
            {
                return customType;
            }

            // handle simple types
            if (IsTsSimpleType(type))
            {
                return GetTsSimpleTypeName(type);
            }

            // handle collection types
            if (IsCollectionType(type))
            {
                return GetTsCollectionTypeName(type);
            }

            // handle dictionaries
            if (IsDictionaryType(type))
            {
                return GetTsDictionaryTypeName(type);
            }

            // handle custom generic types
            if (IsCustomGenericType(type))
            {
                return GetGenericTsTypeName(type, forTypeDeclaration);
            }

            // handle custom types & generic parameters
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
                {
                    throw new CoreException($"No type specified in TsType attribute for member '{memberInfo.Name}' declared in '{memberInfo.DeclaringType?.FullName}'");
                }

                Type type = GetMemberType(memberInfo);
                return GenerateCustomType(type, typeAttribute.TypeName);
            }

            return GetTsTypeNameForMember(memberInfo);
        }

        /// <summary>
        /// Gets TypeScript type name for a member
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when member or typeNameConverters is null</exception>
        private string GetTsTypeNameForMember(MemberInfo memberInfo)
        {
            // special case - dynamic property/field
            
            if (memberInfo.GetCustomAttribute<DynamicAttribute>() != null)
            {
                return "any";
            }

            // otherwise, resolve by type

            Type type = GetMemberType(memberInfo);
            return GetTsTypeName(type);
        }

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

                if (Nullable.GetUnderlyingType(memberType) != null && GeneratorOptions.CsNullableTranslation != StrictNullTypeUnionFlags.None)
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
            // handle IDictionary<,>
            
            Type dictionary2Interface = type.GetInterface("System.Collections.Generic.IDictionary`2");
            if (dictionary2Interface != null || (type.FullName != null && type.FullName.StartsWith("System.Collections.Generic.IDictionary`2")))
            {
                Type dictionaryType = dictionary2Interface ?? type;
                Type keyType = dictionaryType.GetGenericArguments()[0];
                Type valueType = dictionaryType.GetGenericArguments()[1];

                string keyTypeName = GetTsTypeName(keyType);
                string valueTypeName = GetTsTypeName(valueType);

                if (!keyTypeName.In("number", "string"))
                {
                    throw new CoreException($"Error when determining TypeScript type for C# type '{type.FullName}':" +
                                            " TypeScript dictionary key type must be either 'number' or 'string'");
                }

                return GetTsDictionaryTypeText(keyTypeName, valueTypeName);
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
            return GetGenericTsTypeNameDeclarationAgnostic(type,
                t => t.GetTypeInfo().BaseType != null && t.GetTypeInfo().BaseType != typeof(object)
                    ? $"{t.Name} extends {GetTsTypeName(t.GetTypeInfo().BaseType, true)}"
                    : t.Name);
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
        /// Gets a type of a collection element from the given type.
        /// If the passed type is not an array type or does not contain the IEnumerable interface, null is returned.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Type GetTsCollectionElementType(Type type)
        {
            // handle array types
            Type elementType = type.GetElementType();
            if (elementType != null)
            {
                return elementType;
            }

            switch (type.Name)
            {
                // handle IEnumerable<>
                case "IEnumerable`1":
                    return type.GetGenericArguments()[0];
                // handle IEnumerable
                case "IEnumerable":
                    return typeof(object);
            }

            // handle types implementing IEnumerable or IEnumerable<>

            Type ienumerable1Interface = type.GetInterface("IEnumerable`1");
            if (ienumerable1Interface != null) return ienumerable1Interface.GetGenericArguments()[0];
            
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

            Type baseType = type.GetTypeInfo().BaseType;
            if (baseType == null || baseType == typeof(object)) return null;

            if (IsTsClass(type) && IsTsInterface(baseType)) throw new CoreException($"Attempted to generate class '{type.FullName}' which extends an interface '{baseType.FullName}', which is not a valid inheritance chain in TypeScript");

            return baseType;
        }

        /// <inheritdoc />
        public IEnumerable<Type> GetInterfaces(Type type)
        {
            Requires.NotNull(type, nameof(type));

            IEnumerable<Type> baseTypes = type.GetTypeInfo().ImplementedInterfaces;

            // if (IsTsClass(type) && IsTsInterface(baseType)) throw new CoreException($"Attempted to generate class '{type.FullName}' which extends an interface '{baseType.FullName}', which is not a valid inheritance chain in TypeScript");

            return baseTypes;
        }
    }
}
