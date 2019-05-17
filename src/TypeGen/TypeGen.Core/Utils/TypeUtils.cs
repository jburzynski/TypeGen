using System;
using System.Reflection;

namespace TypeGen.Core.Utils
{
    internal static class TypeUtils
    {
        public static object GetDefaultValue(Type type)
        {
            return type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}