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
            string assemblyFilePath = null;

            foreach (string directory in Directories)
            {
                string[] foundPaths = _fileSystem.GetFilesRecursive(directory, assemblyFileName);
                if (!foundPaths.Any()) continue;

                assemblyFilePath = foundPaths.First();
                break;
            }

            if (assemblyFilePath != null) return Assembly.LoadFrom(assemblyFilePath);

            throw new CliException();
        }

        private string GetAssemblyFileName(string assemblyFullName)
        {
            return assemblyFullName.Split(',')[0] + ".dll";
        }
    }
}
