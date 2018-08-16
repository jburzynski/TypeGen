using System;
using System.Collections.Generic;

namespace TypeGen.Core.Business
{
    internal interface ITypeDependencyService
    {
        /// <summary>
        /// Gets all non-simple and non-collection types the given type depends on.
        /// Types of properties/fields marked with TsIgnoreAttribute will be omitted.
        /// Returns an empty array if no dependencies were detected.
        /// Returns a distinct result (i.e. no duplicate TypeDependencyInfo instances)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the type is null</exception>
        IEnumerable<TypeDependencyInfo> GetTypeDependencies(Type type);
    }
}