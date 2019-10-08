using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using TypeGen.Core.Extensions;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Converters
{
    /// <summary>
    /// Converts under_score (or UNDER_SCORE) names to PascalCase names
    /// </summary>
    public class UnderscoreCaseToPascalCaseConverter: IMemberNameConverter, ITypeNameConverter
    {
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
            return name.ToTitleCase();
        }
    }
}
