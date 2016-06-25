using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Core
{
    /// <summary>
    /// Converter that doesn't change the input name
    /// </summary>
    public class NoChangeConverter : INameConverter, ITypeNameConverter
    {
        public string Convert(string name)
        {
            return name;
        }

        public string Convert(string name, Type type)
        {
            return name;
        }
    }
}
