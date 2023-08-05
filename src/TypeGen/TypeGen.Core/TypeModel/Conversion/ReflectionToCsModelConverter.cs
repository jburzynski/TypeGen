#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.Extensions;
using TypeGen.Core.TypeModel.Csharp;
using TypeGen.Core.Validation;

namespace TypeGen.Core.TypeModel.Conversion;

internal class ReflectionToCsModelConverter
{
    public static CsType ConvertType(Type type)
    {
        Requires.NotNull(type, nameof(type));

        if (type.IsNullable())
            return ConvertTypePrivate(Nullable.GetUnderlyingType(type)!, true);
        else
            return ConvertTypePrivate(type, false);
    }

    private static CsType ConvertTypePrivate(Type type, bool isNullable)
        => type switch
        {
            { IsGenericParameter: true } => ConvertGenericParameter(type),
            { IsPrimitive: true } => ConvertPrimitive(type, isNullable),
            { IsEnum: true } => ConvertEnum(type, isNullable),
            { IsClass: true } => ConvertClass(type, isNullable),
            { IsInterface: true } => ConvertInterface(type, isNullable),
            not null when type.IsStruct() =>  ConvertStruct(type, isNullable),
            _ => throw new ArgumentException($"Type '{type!.FullName}' is not supported. Only classes, interfaces, structs, enums and generic parameters can be translated to TypeScript.")
        };

    private static CsGenericParameter ConvertGenericParameter(Type type)
    {
        return new CsGenericParameter(type.Name);
    }
    
    private static CsPrimitive ConvertPrimitive(Type type, bool isNullable)
    {
        return new CsPrimitive(type.FullName, type.Name, isNullable);
    }
    
    private static CsEnum ConvertEnum(Type type, bool isNullable)
    {
        var values = GetEnumValues(type);
        var tgAttributes = GetTgAttributes(type);

        return new CsEnum(type.FullName, type.Name, tgAttributes, values, isNullable);
    }

    private static CsGpType ConvertClass(Type type, bool isNullable)
    {
        var genericTypes = GetGenericTypes(type);
        var @base = GetBase(type);
        var implementedInterfaces = GetImplementedInterfaces(type);
        var tgAttributes = GetTgAttributes(type);
        var fields = GetFields(type);
        var properties = GetProperties(type);

        return CsGpType.Class(type.FullName!,
            type.Name,
            isNullable,
            genericTypes,
            @base,
            implementedInterfaces,
            fields,
            properties,
            tgAttributes);
    }

    private static CsGpType ConvertInterface(Type type, bool isNullable)
    {
        var genericTypes = GetGenericTypes(type);
        var @base = GetBase(type);
        var tgAttributes = GetTgAttributes(type);
        var properties = GetProperties(type);

        return CsGpType.Interface(type.FullName!,
            type.Name,
            isNullable,
            genericTypes,
            @base,
            properties,
            tgAttributes);
    }
    
    private static CsGpType ConvertStruct(Type type, bool isNullable)
    {
        var genericTypes = GetGenericTypes(type);
        var tgAttributes = GetTgAttributes(type);
        var fields = GetFields(type);
        var properties = GetProperties(type);

        return CsGpType.Struct(type.FullName!,
            type.Name,
            isNullable,
            genericTypes,
            fields,
            properties,
            tgAttributes);
    }
    
    private static IReadOnlyCollection<CsEnumValue> GetEnumValues(Type type)
    {
        return type.GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(fieldInfo =>
            {
                var enumValue = fieldInfo.GetValue(null);
                var underlyingValue = Convert.ChangeType(enumValue, Enum.GetUnderlyingType(type));
                return new CsEnumValue(fieldInfo.Name, underlyingValue);
            })
            .ToList();
    }

    private static IReadOnlyCollection<CsType> GetGenericTypes(Type type)
    {
        var reflectionGenericTypes = type.IsGenericTypeDefinition
            ? type.GetTypeInfo().GenericTypeParameters
            : type.GenericTypeArguments;
        return reflectionGenericTypes.Select(ConvertType).ToList();
    }

    private static CsGpType? GetBase(Type type)
        => type.BaseType != null ? (CsGpType)ConvertType(type.BaseType) : null;
    
    private static IReadOnlyCollection<CsGpType> GetImplementedInterfaces(Type type)
        => type.GetInterfaces()
            .Select(ConvertType)
            .Cast<CsGpType>()
            .ToList();
    
    private static IReadOnlyCollection<Attribute> GetTgAttributes(Type type)
        => type.GetCustomAttributes().GetTypeGenAttributes().ToList();
    
    private static IReadOnlyCollection<CsProperty> GetProperties(Type type)
        => type.GetProperties()
            .Select(propertyInfo =>
            {
                var propertyType = ConvertType(propertyInfo.PropertyType);
                var name = propertyInfo.Name;
                var defaultValue = propertyInfo.GetValue(null);
                return new CsProperty(propertyType, name, defaultValue);
            })
            .ToList();
    
    private static IReadOnlyCollection<CsField> GetFields(Type type)
    {
        return type.GetFields()
            .Select(fieldInfo =>
            {
                var fieldType = ConvertType(fieldInfo.FieldType);
                var name = fieldInfo.Name;
                var defaultValue = fieldInfo.GetValue(null);
                return new CsField(fieldType, name, defaultValue);
            })
            .ToList();
    }
}