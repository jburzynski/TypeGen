using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core.Converters
{
    /// <summary>
    /// Represents a collection of name converters
    /// </summary>
    public class NameConverterCollection : IEnumerable<INameConverter>
    {
        private readonly IList<INameConverter> _converters;

        public NameConverterCollection()
        {
            _converters = new List<INameConverter>();
        }

        public NameConverterCollection(params INameConverter[] converters)
            : this((IEnumerable<INameConverter>) converters)
        {
        }

        public NameConverterCollection(IEnumerable<INameConverter> converters)
        {
            _converters = converters.ToList();
        }

        /// <summary>
        /// Adds a converter to the collection. Null converters will not be added.
        /// </summary>
        /// <param name="converter"></param>
        public void Add(INameConverter converter)
        {
            if (converter == null) return;
            _converters.Add(converter);
        }

        /// <summary>
        /// Removes a converter from the collection.
        /// Throws an exception if attempted to remove the first, default NoChangeConverter converter.
        /// </summary>
        /// <param name="converter"></param>
        public void Remove(INameConverter converter)
        {
            if (_converters.IndexOf(converter) == 0)
            {
                throw new CoreException("Cannot remove the first, default NoChangeConverter");
            }

            _converters.Remove(converter);
        }

        /// <summary>
        /// Converts a name using the chain of converters
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Convert(string name)
        {
            return _converters.Aggregate(name, (current, converter) => converter.Convert(current));
        }

        public IEnumerator<INameConverter> GetEnumerator()
        {
            return _converters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
