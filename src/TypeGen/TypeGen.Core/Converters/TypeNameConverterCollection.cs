using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Converters
{
    /// <summary>
    /// Represents a collection of type name converters
    /// </summary>
    public class TypeNameConverterCollection : IEnumerable<ITypeNameConverter>
    {
        private readonly IList<ITypeNameConverter> _converters;

        public TypeNameConverterCollection()
        {
            _converters = new List<ITypeNameConverter>();
        }

        public TypeNameConverterCollection(params ITypeNameConverter[] converters)
            : this((IEnumerable<ITypeNameConverter>) converters)
        {
        }

        public TypeNameConverterCollection(IEnumerable<ITypeNameConverter> converters)
        {
            _converters = converters.ToList();
        }

        /// <summary>
        /// Adds a type converter to the collection.
        /// </summary>
        /// <param name="converter"></param>
        public void Add(ITypeNameConverter converter)
        {
            Requires.NotNull(converter, nameof(converter));
            _converters.Add(converter);
        }

        /// <summary>
        /// Removes a type converter from the collection.
        /// </summary>
        /// <param name="converter"></param>
        public void Remove(ITypeNameConverter converter)
        {
            Requires.NotNull(converter, nameof(converter));
            _converters.Remove(converter);
        }

        /// <summary>
        /// Converts a type name using the chain of converters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string Convert(string name, Type type)
        {
            return _converters.Aggregate(name, (current, converter) => converter.Convert(current, type));
        }

        public IEnumerator<ITypeNameConverter> GetEnumerator()
        {
            return _converters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
