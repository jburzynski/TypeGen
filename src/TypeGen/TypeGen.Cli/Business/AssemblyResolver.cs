using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Cli.Business
{
    internal class AssemblyResolver
    {
        private readonly FileSystem _fileSystem;

        public IEnumerable<string> Directories { get; set; }

        public AssemblyResolver(FileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void Register()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }

        public void Unregister()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;
        }

        private Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyFileName = GetAssemblyFileName(args.Name);

            foreach (string directory in Directories)
            {
                string[] foundPaths = _fileSystem.GetFilesRecursive(directory, assemblyFileName);
                if (!foundPaths.Any()) continue;

                Assembly assembly = ResolveFromPaths(foundPaths);
                if (assembly != null) return assembly;
            }

            throw new AssemblyResolutionException($"Could not resolve assembly: {args.Name} in any of the searched directories: {string.Join("; ", Directories)}");
        }

        private Assembly ResolveFromPaths(IEnumerable<string> paths)
        {
            foreach (string path in paths)
            {
                try
                {
                    return Assembly.LoadFrom(path);
                }
                catch (BadImageFormatException)
                {
                }
            }

            return null;
        }

        private string GetAssemblyFileName(string assemblyFullName)
        {
            return assemblyFullName.Split(',')[0] + ".dll";
        }
    }
}
