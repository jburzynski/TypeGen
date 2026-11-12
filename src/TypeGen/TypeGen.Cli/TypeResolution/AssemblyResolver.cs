using NuGet.Configuration;
using NuGet.Frameworks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TypeGen.Cli.Extensions;
using TypeGen.Core.Logging;

namespace TypeGen.Cli.TypeResolution
{
    internal class AssemblyResolver : IDisposable
    {
        private static readonly Regex _assemblyVersionRegex = new(@"Version=(.+?),");

        private static readonly char _sep = Path.DirectorySeparatorChar;
        private readonly string[] _additionalAssemblyDirectories;
        private readonly string[] _additionalAssemblyFilePaths;
        private readonly IReadOnlyList<NuGetFramework> _assembliesFrameworks;

        private readonly string _globalFallbackPath;
        private readonly ILogger _logger;
        private readonly string _projectFolder;
        private readonly string _sharedFolder;
        private List<string> _nugetPackageFolders = [];

        private readonly ConcurrentDictionary<string, Assembly> _resolutionCache = new();

        public AssemblyResolver(string projectFolder,
            IReadOnlyList<NuGetFramework> assembliesFrameworks,
            string[] additionalAssemblyDirectories,
            string[] additionalAssemblyFilePaths,
            ILogger logger)
        {
            _logger = logger;
            _assembliesFrameworks = assembliesFrameworks;
            _additionalAssemblyDirectories = additionalAssemblyDirectories;
            _additionalAssemblyFilePaths = additionalAssemblyFilePaths;
            _projectFolder = projectFolder.ToAbsolutePath();
            
            var dotnetInstallPath = GetDotnetInstallPath();
            _globalFallbackPath = Path.Combine(dotnetInstallPath, "sdk/NuGetFallbackFolder");
            var dotNetInstallSharedPath = Path.Combine(dotnetInstallPath, "shared");

            if (Directory.Exists(dotNetInstallSharedPath)) _sharedFolder = dotNetInstallSharedPath;

            PopulateNuGetPackageFolders();
            Register();
        }

        private void PopulateNuGetPackageFolders()
        {
            NuGetPathContext nugetPathContext = NuGetPathContext.Create(_projectFolder);

            if (!string.IsNullOrWhiteSpace(nugetPathContext.UserPackageFolder)) _nugetPackageFolders.Add(nugetPathContext.UserPackageFolder);
            _nugetPackageFolders.AddRange(nugetPathContext.FallbackPackageFolders);
            if (!_nugetPackageFolders.Contains(_globalFallbackPath) && Directory.Exists(_globalFallbackPath)) _nugetPackageFolders.Add(_globalFallbackPath);
        }

        private void Register()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }

        private void Unregister()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;
        }

        private Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            _logger?.Log($"Attempting to resolve assembly '{args.Name}'...", LogLevel.Debug);

            if (args.Name.Split(',')[0].EndsWith(".resources"))
            {
                _logger?.Log($"Skipping resolution of assembly '{args.Name}', because it is a resource assembly.", LogLevel.Debug);
                return null;
            }

            var result = _resolutionCache.GetOrAdd(args.Name, assemblyName => GetAssembly(assemblyName));
            if (result == null) _logger?.Log($"Could not resolve assembly '{args.Name}'.", LogLevel.Debug);

            return result;
        }

        private Assembly GetAssembly(string assemblyName)
        {
            var assemblyShortName = GetAssemblyShortName(assemblyName);
            var assemblyVersion = GetAssemblyVersion(assemblyName);
            HashSet<string> foundPaths = [];

            var assembly = FindInAdditionalAssemblyFilePaths(assemblyShortName, assemblyName, assemblyVersion, foundPaths);
            if (assembly != null) return assembly;

            assembly = FindByPackageName(_nugetPackageFolders, assemblyName, foundPaths);
            if (assembly != null) return assembly;

            assembly = FindByPackageName(_additionalAssemblyDirectories, assemblyName, foundPaths);
            if (assembly != null) return assembly;

            if (_sharedFolder != null)
            {
                assembly = FindByAssemblyName(_sharedFolder, assemblyShortName, assemblyName, assemblyVersion, foundPaths);
                if (assembly != null) return assembly;
            }

            assembly = FindByAssemblyName(_additionalAssemblyDirectories, assemblyShortName, assemblyName, assemblyVersion, foundPaths);
            if (assembly != null) return assembly;

            if (foundPaths.Count > 0) assembly = ResolveFromPaths(foundPaths, assemblyName, assemblyVersion);

            if (assembly == null)
            {
                var searchedDirectories = _additionalAssemblyDirectories.Concat(_nugetPackageFolders).Concat(new[] { _sharedFolder }).ToList();
                _logger?.Log($"Could not resolve assembly '{assemblyName}' in any of the searched directories: {string.Join("; ", searchedDirectories)}", LogLevel.Error);
            }

            return assembly;
        }

        private Assembly FindByPackageName(IEnumerable<string> directories,
            string assemblyFullName,
            HashSet<string> foundPaths)
        {
            return directories
                .Select(directory => FindByPackageName(directory, assemblyFullName, foundPaths))
                .FirstOrDefault(assembly => assembly != null);
        }

        private Assembly FindByPackageName(string directory, string assemblyFullName, HashSet<string> foundPaths)
        {
            var assemblyShortName = GetAssemblyShortName(assemblyFullName);
            var assemblyVersion = GetAssemblyVersion(assemblyFullName);

            var packageFolder = GetPackageFolder(directory, assemblyShortName);

            return packageFolder != null ?
                FindByAssemblyName(packageFolder, assemblyShortName, assemblyFullName, assemblyVersion, foundPaths) :
                null;
        }

        private Assembly FindByAssemblyName(IEnumerable<string> directories,
            string assemblyShortName,
            string assemblyFullName,
            Version targetVersion,
            HashSet<string> foundPaths)
        {
            return directories.Select(directory => FindByAssemblyName(directory, assemblyShortName, assemblyFullName, targetVersion, foundPaths))
                .FirstOrDefault(assembly => assembly != null);
        }

        private Assembly FindInAdditionalAssemblyFilePaths(string assemblyShortName,
            string assemblyFullName,
            Version targetVersion,
            HashSet<string> foundPaths)
        {
            var matchingPaths = _additionalAssemblyFilePaths
                .Where(x => Path.GetFileNameWithoutExtension(x).Equals(assemblyShortName, StringComparison.Ordinal))
                .ToList();

            foundPaths.UnionWith(matchingPaths);

            // Leaving this in case early returns are ever needed for optimization.
            // Currently searching all directories takes very little time,
            // so there's no need to optimize any further.
            return null;
        }

        private Assembly FindByAssemblyName(string directory,
            string assemblyShortName,
            string assemblyFullName,
            Version targetVersion,
            HashSet<string> foundPaths)
        {
            var patterns = new[]
            {
                $"{assemblyShortName}.dll",
                $"{assemblyShortName}.exe"
            };

            foreach (var pattern in patterns)
            {
                var found = Directory.GetFiles(directory, pattern, SearchOption.AllDirectories)
                    .Where(x => !x.Contains($"{_sep}obj{_sep}", StringComparison.OrdinalIgnoreCase));

                foundPaths.UnionWith(found);
            }

            // Leaving this in case early returns are ever needed for optimization.
            // Currently searching all directories takes very little time,
            // so there's no need to optimize any further.
            return null;
        }

        private string GetAssemblyShortName(string assemblyFullName)
        {
            return assemblyFullName.Split(',')[0];
        }

        private Version GetAssemblyVersion(string assemblyFullName)
        {
            var match = _assemblyVersionRegex.Match(assemblyFullName);
            return match.Success && Version.TryParse(match.Groups[1].Value, out var version) ? version : null;
        }

        private string GetDotnetInstallPath()
        {
            string programFiles = Environment.GetEnvironmentVariable("programfiles");
            if (!string.IsNullOrWhiteSpace(programFiles))
            {
                string dotnetDirWin64 = Path.Combine(programFiles, "dotnet");
                if (Directory.Exists(dotnetDirWin64)) return dotnetDirWin64;
            }

            string programFilesX86 = Environment.GetEnvironmentVariable("programfiles(x86)");
            if (!string.IsNullOrWhiteSpace(programFilesX86))
            {
                string dotnetDirWinX86 = Path.Combine(programFilesX86, "dotnet");
                if (Directory.Exists(dotnetDirWinX86)) return dotnetDirWinX86;
            }

            var osxPath = "/usr/local/share/dotnet";
            if (Directory.Exists(osxPath)) return osxPath;

            var linuxPath = "/usr/share/dotnet";
            if (Directory.Exists(linuxPath)) return linuxPath;

            return @"C:\Program Files\dotnet";
        }

        private string GetPackageFolder(string root, string assemblyFolder)
        {
            string packageFolder = Path.Combine(root, assemblyFolder);
            if (Directory.Exists(packageFolder)) return packageFolder;

            string[] assemblyFolderParts = assemblyFolder.Split('.');
            if (assemblyFolderParts.Length > 1)
            {
                string truncatedAssemblyFolder = string.Join('.', assemblyFolderParts.Take(assemblyFolderParts.Length - 1));
                return GetPackageFolder(root, truncatedAssemblyFolder);
            }

            return null;
        }

        private Assembly ResolveFromPaths(IReadOnlyCollection<string> paths,
            string assemblyFullName,
            Version targetVersion)
        {
            if (paths.Count == 0) return null;

            List<AssemblyCandidate> candidates = paths
                .Select(x =>
                {
                    var assemblyFramework = TargetFrameworkUtils.GetAssemblyTargetFramework(x);
                    var assemblyVersion = TryGetAssemblyNameFromPath(x, out var assemblyName) ? assemblyName!.Version : null;

                    return (path: x, assemblyFramework, assemblyVersion);
                })
                .Where(x => x.assemblyFramework != null && x.assemblyVersion != null && !x.assemblyFramework.IsUnsupported &&
                    DefaultCompatibilityProvider.Instance.IsCompatible(Constants.CliFramework, x.assemblyFramework))
                .Select(x => new AssemblyCandidate(x.path, x.assemblyVersion!, x.assemblyFramework!))
                .ToList();

            if (candidates.Count == 0) return null;

            AssemblyCandidate bestWithEqualOrHigherVersion = default;

            if (targetVersion != null)
            {
                bestWithEqualOrHigherVersion = candidates
                    .OrderBy(x => x.Version)
                    .FirstOrDefault(x => x.Version >= targetVersion);

                if (bestWithEqualOrHigherVersion != default)
                    candidates = candidates.Where(x => x.Version == bestWithEqualOrHigherVersion.Version).ToList();
            }

            if (candidates.Count > 1)
            {
                candidates = candidates
                .OrderByDescending(x => _assembliesFrameworks.Any(f => DefaultCompatibilityProvider.Instance.IsCompatible(f, x.Framework)))
                .ThenByDescending(x => x.Version)
                .ThenBy(x => TargetFrameworkUtils.GetFrameworkFamilyRank(x.Framework))
                .ThenByDescending(x => x.Framework.Version)
                .ToList();
            }

            foreach (var candidate in candidates)
            {
                if (!TryLoadAssemblyFromPath(candidate.Path, out var assembly)) continue;

                _logger?.Log($"Assembly '{assemblyFullName}' resolved to version '{assembly!.GetName().Version}', framework '{candidate.Framework}' from file '{candidate.Path}'.", LogLevel.Debug);
                return assembly;
            }

            return null;
        }

        private bool TryGetAssemblyNameFromPath(string path, out AssemblyName assemblyName)
        {
            assemblyName = null;

            try
            {
                assemblyName = AssemblyName.GetAssemblyName(path);
                return true;
            }
            catch (Exception e)
            {
                _logger?.Log($"Error while reading assembly name from path: '{path}'. Exception: {e}", LogLevel.Debug);
                return false;
            }
        }

        private bool TryLoadAssemblyFromPath(string path, out Assembly assembly)
        {
            assembly = null;

            try
            {
                assembly = Assembly.LoadFrom(path);
                return true;
            }
            catch (Exception e)
            {
                _logger?.Log($"Error while loading assembly from path: '{path}'. Exception: {e}", LogLevel.Debug);
                return false;
            }
        }

        ~AssemblyResolver() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            Unregister();
        }

        private record struct AssemblyCandidate(string Path, Version Version, NuGetFramework Framework);
    }
}
