using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TypeGen.Core.Business;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Extensions
{
    internal static class AssemblyExtensions
    {
        /// <summary>
        /// Gets all types that can be loaded from an assembly.
        /// Source: http://stackoverflow.com/questions/11915389/assembly-gettypes-throwing-an-exception
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            Requires.NotNull(assembly, nameof(assembly));

            try
            {
                return assembly.DefinedTypes
                    .Where(ti => ti?.Namespace != null) // skip anonymous types
                    .Select(ti => ti.AsType());
            }
            catch (ReflectionTypeLoadException e)
            {
                IEnumerable<Type> types = e.Types.Where(t => t != null);

                if (!types.Any()) throw new AssemblyResolutionException($"Could not resolve assembly '{assembly.FullName}'");

                return types;
            }
        }
    }
}
