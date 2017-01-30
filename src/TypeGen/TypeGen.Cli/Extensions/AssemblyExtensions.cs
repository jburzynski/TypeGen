using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Cli.Extensions
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
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
