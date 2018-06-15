using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NuGet.Configuration;
using TypeGen.Cli.Extensions;
using TypeGen.Core.Storage;
using TypeGen.Core.Business;

namespace TypeGen.Cli.Business
{
    internal class AssemblyResolver
    {
        private const string GlobalFallbackPath = @"C:\Program Files\dotnet\sdk\NuGetFallbackFolder";
        private const string SharedPath = @"C:\Program Files\dotnet\shared";

        private readonly FileSystem _fileSystem;
        private readonly string _projectFolder;

        private IEnumerable<string> _directories;
        public IEnumerable<string> Directories
        {
            get => _directories;
            set => _directories = value?.Select(d => Path.IsPathRooted(d) ? d : Path.Combine(_projectFolder, d));
        }

        private string _sharedFolder;
        private List<string> _nugetPackagesFolders;

        public AssemblyResolver(FileSystem fileSystem, string projectFolder)
        {
            _fileSystem = fileSystem;
            _projectFolder = projectFolder.ToAbsolutePath();

            if (Directory.Exists(SharedPath)) _sharedFolder = SharedPath;
            PopulateNuGetPackageFolders();
        }

        private void PopulateNuGetPackageFolders()
        {
            NuGetPathContext nugetPathContext = NuGetPathContext.Create(_projectFolder);
            _nugetPackagesFolders = new List<string>();

            if (!string.IsNullOrWhiteSpace(nugetPathContext.UserPackageFolder)) _nugetPackagesFolders.Add(nugetPathContext.UserPackageFolder);
            _nugetPackagesFolders.AddRange(nugetPathContext.FallbackPackageFolders);
            if (!_nugetPackagesFolders.Contains(GlobalFallbackPath) && Directory.Exists(GlobalFallbackPath)) _nugetPackagesFolders.Add(GlobalFallbackPath);
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
            // step 1 - search by assembly name (nuget global + nuget fallback + user-defined)

            // nuget
            Assembly assembly = FindByPackageName(_nugetPackagesFolders, args.Name);
            if (assembly != null) return assembly;

            // user-defined
            assembly = FindByPackageName(Directories, args.Name);
            if (assembly != null) return assembly;

            // step 2 - search recursively (shared + user-defined + nuget global + nuget fallback)

            string assemblyFileName = GetAssemblyFileName(args.Name);
            string assemblyVersion = GetAssemblyVersion(args.Name);

            // shared
            assembly = FindRecursive(_sharedFolder, assemblyFileName, assemblyVersion);
            if (assembly != null) return assembly;

            // user-defined
            assembly = FindRecursive(Directories, assemblyFileName, assemblyVersion);
            if (assembly != null) return assembly;

            // nuget
            assembly = FindRecursive(_nugetPackagesFolders, assemblyFileName, assemblyVersion);
            if (assembly != null) return assembly;

            // exception if assembly not found

            throw new AssemblyResolutionException($"Could not resolve assembly: {args.Name} in any of the searched directories: {string.Join("; ", Directories)}");
        }

        private Assembly FindByPackageName(IEnumerable<string> directories, string assemblyFullName)
        {
            return directories.Select(directory => FindByPackageName(directory, assemblyFullName)).FirstOrDefault(assembly => assembly != null);
        }

        private Assembly FindByPackageName(string directory, string assemblyFullName)
        {
            string assemblyShortName = GetAssemblyShortName(assemblyFullName);
            string assemblyFileName = GetAssemblyFileName(assemblyFullName);
            string assemblyVersion = GetAssemblyVersion(assemblyFullName);

            string packageFolder = GetPackageFolder(directory, assemblyShortName);
            return packageFolder == null ? null : FindRecursive(packageFolder, assemblyFileName, assemblyVersion);
        }

        private string GetPackageFolder(string root, string assemblyFolder)
        {
            string packageFolder = Path.Combine(root, assemblyFolder);
            if (_fileSystem.DirectoryExists(packageFolder)) return packageFolder;

            string[] assemblyFolderParts = assemblyFolder.Split('.');
            if (assemblyFolderParts.Length > 1)
            {
                string truncatedAssemblyFolder = string.Join('.', assemblyFolderParts.Take(assemblyFolderParts.Length - 1));
                return GetPackageFolder(root, truncatedAssemblyFolder);
            }

            return null;
        }

        private Assembly FindRecursive(IEnumerable<string> directories, string assemblyFileName, string assemblyVersion)
        {
            return directories.Select(directory => FindRecursive(directory, assemblyFileName, assemblyVersion)).FirstOrDefault(assembly => assembly != null);
        }

        private Assembly FindRecursive(string directory, string assemblyFileName, string assemblyVersion)
        {
            string[] foundPaths = _fileSystem.GetFilesRecursive(directory, assemblyFileName);
            return foundPaths.Any() ? ResolveFromPaths(foundPaths, assemblyVersion) : null;
        }

        private Assembly ResolveFromPaths(IEnumerable<string> paths, string assemblyVersion)
        {
            foreach (string path in paths)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(path);
                    if (assembly.GetName().Version.ToString() == assemblyVersion) return assembly;
                }
                catch (BadImageFormatException)
                {
                }
            }

            return null;
        }

        private string GetAssemblyShortName(string assemblyFullName)
        {
            return assemblyFullName.Split(',')[0];
        }

        private string GetAssemblyFileName(string assemblyFullName)
        {
            return GetAssemblyShortName(assemblyFullName) + ".dll";
        }

        private string GetAssemblyVersion(string assemblyFullName)
        {
            Match match = Regex.Match(assemblyFullName, "Version=(.+?),");
            return match.Success ? match.Groups[1].Value : throw new CliException($"Could not determine assembly version of assembly: ${assemblyFullName}");
        }
    }
}
