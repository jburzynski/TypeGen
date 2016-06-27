using System;

namespace TypeGen.Core.Converters
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
