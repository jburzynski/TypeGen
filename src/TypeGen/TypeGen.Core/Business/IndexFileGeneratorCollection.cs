using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core.Business
{
    /// <summary>
    /// Represents a collection of index file generators
    /// </summary>
    public class IndexFileGeneratorCollection : IEnumerable<IIndexFileGenerator>
    {
        private readonly IList<IIndexFileGenerator> _generators;

        public IndexFileGeneratorCollection()
        {
            _generators = new List<IIndexFileGenerator>();
        }

        public IndexFileGeneratorCollection(params IIndexFileGenerator[] converters)
            : this((IEnumerable<IIndexFileGenerator>)converters)
        {
        }

        public IndexFileGeneratorCollection(IEnumerable<IIndexFileGenerator> converters)
        {
            _generators = converters.ToList();
        }

        /// <summary>
        /// Adds a index file generator to the collection. Null index file generators will not be added.
        /// </summary>
        /// <param name="converter"></param>
        public void Add(IIndexFileGenerator converter)
        {
            if (converter == null) return;
            _generators.Add(converter);
        }

        /// <summary>
        /// Removes a index file generator from the collection.
        /// </summary>
        /// <param name="converter"></param>
        public void Remove(IIndexFileGenerator converter)
        {
            _generators.Remove(converter);
        }

        /// <summary>
        /// Creates index files using the chain of index file generators
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public IEnumerable<string> Generate(IEnumerable<string> files)
        {
            var results = new List<string>();
            foreach (var generator in _generators)
            {
                results.AddRange(generator.Generate(files));
            }
            return results;
        }

        public IEnumerator<IIndexFileGenerator> GetEnumerator()
        {
            return _generators.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
    }
}
