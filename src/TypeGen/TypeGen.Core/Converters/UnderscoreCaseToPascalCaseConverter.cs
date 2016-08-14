using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TypeGen.Core.Extensions;

namespace TypeGen.Core.Converters
{
    /// <summary>
    /// Converts under_score (or UNDER_SCORE) names to PascalCase names
    /// </summary>
    public class UnderscoreCaseToPascalCaseConverter: INameConverter, ITypeNameConverter
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
            return name.ToTitleCase(CultureInfo.InvariantCulture).Replace("_", "");
        }
    }
}
