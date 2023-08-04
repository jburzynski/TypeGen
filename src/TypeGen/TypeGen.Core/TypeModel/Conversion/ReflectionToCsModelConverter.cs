using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.Extensions;
using TypeGen.Core.Metadata;
using TypeGen.Core.TypeModel.Csharp;
using TypeGen.Core.TypeModel.TypeScript;
using TypeGen.Core.Validation;

namespace TypeGen.Core.TypeModel.Conversion;

internal class ReflectionToCsModelConverter
{
    public static CsType ConvertType(Type type)
    {
        Requires.NotNull(type, nameof(type));

        if (type.IsNullable())
            return ConvertTypePrivate(Nullable.GetUnderlyingType(type), true);
        else
            return ConvertTypePrivate(type, false);
    }

    private static CsType ConvertTypePrivate(Type type, bool isNullable)
    {
        if (type.IsGenericParameter)
            return ConvertGenericParameter(type);

        if (type.IsEnum)
            return ConvertEnum(type, isNullable);

        if (type.IsClass)
            return ConvertClass(type, isNullable);

        return null;
    }

    private static CsGenericParameter ConvertGenericParameter(Type type)
    {
        return new CsGenericParameter(type.Name);
    }
    
    private static CsEnum ConvertEnum(Type type, bool isNullable)
    {
        var values = type.GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(fieldInfo =>
            {
                var enumValue = fieldInfo.GetValue(null);
                var underlyingValue = Convert.ChangeType(enumValue, Enum.GetUnderlyingType(type));
                return new CsEnumValue(fieldInfo.Name, underlyingValue);
            })
            .ToList();

        var tgAttributes = type.GetCustomAttributes().GetTypeGenAttributes().ToList();

        return new CsEnum(type.FullName, type.Name, tgAttributes, values, isNullable);
    }
    
    private static CsGpType ConvertClass(Type type, bool isNullable)
    {
        var reflectionGenericTypes = type.IsGenericTypeDefinition
            ? type.GetTypeInfo().GenericTypeParameters
            : type.GenericTypeArguments;
        var genericTypes = reflectionGenericTypes.Select(ConvertType);
        
        var tgAttributes = type.GetCustomAttributes().GetTypeGenAttributes();

        var properties = type.GetProperties()
            .Select(propertyInfo =>
            {
                var propertyType = ConvertType(propertyInfo.PropertyType);
                var name = propertyInfo.Name;
                var defaultValue = propertyInfo.GetValue(null);
                return new CsProperty(propertyType, name, defaultValue);
            })
            .ToList();

        var fields = type.GetFields()
            .Select(fieldInfo =>
            {
                var fieldType = ConvertType(fieldInfo.FieldType);
                var name = fieldInfo.Name;
                var defaultValue = fieldInfo.GetValue(null);
                return new CsField(fieldType, name, defaultValue);
            })
            .ToList();

        return null;
    }
}