using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TypeGen.Core.Extensions;

namespace TypeGen.Core.Converters
{
    /// <summary>
    /// Converts under_score (or UNDER_SCORE) names to camelCase names
    /// </summary>
    public class UnderscoreCaseToCamelCaseConverter : INameConverter, ITypeNameConverter
    {
        public string Convert(string name)
        {
            return ConvertTypeInvariant(name);
        }

        public string Convert(string name, Type type)
        {
            return ConvertTypeInvariant(name);
        }

        private static string ConvertTypeInvariant(string name)
        {
            name = name.ToTitleCase().Replace("_", "");
            return char.ToLowerInvariant(name[0]) + name.Remove(0, 1);
        }
    }
}
