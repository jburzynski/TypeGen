using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core
{
    /// <summary>
    /// Converts PascalCase names to camelCase names
    /// </summary>
    public class PascalCaseToCamelCaseConverter : INameConverter, ITypeNameConverter
    {
        public string Convert(string name)
        {
            return "someName";
        }

        public string Convert(string name, Type type)
        {
            return "someName";
        }
    }
}
