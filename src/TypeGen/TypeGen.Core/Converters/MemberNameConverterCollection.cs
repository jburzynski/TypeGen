using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Converters
{
    /// <summary>
    /// Represents a collection of member name converters
    /// </summary>
    public class MemberNameConverterCollection : IEnumerable<IMemberNameConverter>
    {
        private readonly IList<IMemberNameConverter> _converters;

        public MemberNameConverterCollection()
        {
            _converters = new List<IMemberNameConverter>();
        }

        public MemberNameConverterCollection(params IMemberNameConverter[] converters)
            : this((IEnumerable<IMemberNameConverter>) converters)
        {
        }

        public MemberNameConverterCollection(IEnumerable<IMemberNameConverter> converters)
        {
            _converters = converters.ToList();
        }

        /// <summary>
        /// Adds a converter to the collection.
        /// </summary>
        /// <param name="converter"></param>
        public void Add(IMemberNameConverter converter)
        {
            Requires.NotNull(converter, nameof(converter));
            _converters.Add(converter);
        }

        /// <summary>
        /// Removes a converter from the collection.
        /// </summary>
        /// <param name="converter"></param>
        public void Remove(IMemberNameConverter converter)
        {
            Requires.NotNull(converter, nameof(converter));
            _converters.Remove(converter);
        }

        /// <summary>
        /// Converts a name using the chain of converters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public string Convert(string name, MemberInfo memberInfo)
        {
            return _converters.Aggregate(name, (current, converter) => converter.Convert(current, memberInfo));
        }

        public IEnumerator<IMemberNameConverter> GetEnumerator()
        {
            return _converters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
