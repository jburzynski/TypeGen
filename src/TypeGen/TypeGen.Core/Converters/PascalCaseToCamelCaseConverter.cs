using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Converters
{
    /// <summary>
    /// Converts PascalCase names to camelCase names
    /// </summary>
    public class PascalCaseToCamelCaseConverter : IMemberNameConverter, ITypeNameConverter
    {
        private static readonly CamelCasePropertyNamesContractResolver _resolver = new();
        public string Convert(string name, MemberInfo memberInfo)
        {
            Requires.NotNullOrEmpty(name, nameof(name));
            return ConvertTypeInvariant(name);
        }

        public string Convert(string name, Type type)
        {
            Requires.NotNullOrEmpty(name, nameof(name));
            return ConvertTypeInvariant(name);
        }

        private static string ConvertTypeInvariant(string name)
        {
            return _resolver.GetResolvedPropertyName(name);
        }
    }
}
