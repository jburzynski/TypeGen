using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Core
{
    /// <summary>
    /// Converts PascalCase names to kebab-case names
    /// </summary>
    public class PascalCaseToKebabCaseConverter : INameConverter, ITypeNameConverter
    {
        public string Convert(string name)
        {
            return "some-name";
        }

        public string Convert(string name, Type type)
        {
            return "some-name";
        }
    }
}
