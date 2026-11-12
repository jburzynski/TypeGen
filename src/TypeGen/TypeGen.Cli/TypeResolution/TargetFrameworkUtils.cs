using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using static NuGet.Frameworks.FrameworkConstants;

namespace TypeGen.Cli.TypeResolution;

internal static class TargetFrameworkUtils
{
    /// <summary>
    /// Parses a .csproj file and returns a list of canonical target strings 
    /// that can be directly compared against the value of TargetFrameworkAttribute.
    /// Example outputs: ".NETCoreApp,Version=v10.0", ".NETFramework,Version=v4.8", ".NETStandard,Version=v2.0"
    /// </summary>
    public static IReadOnlyList<NuGetFramework> GetProjectTargetFrameworks(string projectFilePath)
    {
        if (!File.Exists(projectFilePath))
            throw new FileNotFoundException("The specified project file was not found.", projectFilePath);

        try
        {
            var doc = XDocument.Load(projectFilePath);
            var targets = new List<NuGetFramework>();

            foreach (var propertyGroup in doc.Descendants().Where(x => x.Name.LocalName == "PropertyGroup"))
            {
                // 1. Single modern TargetFramework (e.g., net10.0, netstandard2.0)
                AddFramework(propertyGroup.Elements().FirstOrDefault(x => x.Name.LocalName == "TargetFramework")?.Value, targets);

                // 2. Multi-targeting TargetFrameworks (e.g., net8.0;net10.0)
                var targetFrameworks = propertyGroup.Elements().FirstOrDefault(x => x.Name.LocalName == "TargetFrameworks")?.Value;
                if (!string.IsNullOrWhiteSpace(targetFrameworks))
                {
                    var targetFrameworkItems = targetFrameworks.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var tf in targetFrameworkItems)
                        AddFramework(tf, targets);
                }

                // 3. .NET Framework TargetFrameworkVersion (e.g., v4.8)
                var targetFrameworkVersion = propertyGroup.Elements().FirstOrDefault(x => x.Name.LocalName == "TargetFrameworkVersion")?.Value;
                if (!string.IsNullOrWhiteSpace(targetFrameworkVersion))
                {
                    // NuGetFramework can parse version strings if formatted as a framework or version
                    AddFramework($"netframework{targetFrameworkVersion.TrimStart('v', 'V')}", targets);
                }
            }

            return targets.DistinctBy(x => x.DotNetFrameworkName).ToList();
        }
        catch (Exception e)
        {
            throw new CliException($"An error occurred while parsing project file '{projectFilePath}'.", e);
        }
    }

    private static void AddFramework(string rawFramework, List<NuGetFramework> targets)
    {
        if (string.IsNullOrWhiteSpace(rawFramework)) return;

        var parsed = NuGetFramework.Parse(rawFramework.Trim());
        if (parsed.IsUnsupported) return;

        targets.Add(parsed);
    }

    public static NuGetFramework GetAssemblyTargetFramework(string assemblyPath)
    {
        if (!File.Exists(assemblyPath)) return null;

        try
        {
            using var stream = File.OpenRead(assemblyPath);
            using var peReader = new PEReader(stream);

            if (!peReader.HasMetadata) return null;

            var metadataReader = peReader.GetMetadataReader();

            foreach (var handle in metadataReader.CustomAttributes)
            {
                var attribute = metadataReader.GetCustomAttribute(handle);

                if (attribute.Constructor.Kind == HandleKind.MemberReference)
                {
                    var memberRef = metadataReader.GetMemberReference((MemberReferenceHandle)attribute.Constructor);
                    if (memberRef.Parent.Kind == HandleKind.TypeReference)
                    {
                        var typeRef = metadataReader.GetTypeReference((TypeReferenceHandle)memberRef.Parent);
                        string typeName = metadataReader.GetString(typeRef.Name);
                        string namespaceName = metadataReader.GetString(typeRef.Namespace);

                        if (typeName == "TargetFrameworkAttribute" && namespaceName == "System.Runtime.Versioning")
                        {
                            var attributeBlobReader = metadataReader.GetBlobReader(attribute.Value);

                            // Custom attribute blob format starts with a 16-bit prolog (usually 0x0001)
                            if (attributeBlobReader.Length >= 2 && attributeBlobReader.ReadUInt16() == 0x0001)
                            {
                                // e.g., ".NETCoreApp,Version=v10.0" or ".NETFramework,Version=v4.8"
                                string canonical = attributeBlobReader.ReadSerializedString();
                                if (string.IsNullOrWhiteSpace(canonical)) return null;

                                var framework = NuGetFramework.Parse(canonical);
                                if (framework.IsUnsupported) return null;

                                return framework;
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            // Ignore invalid PE headers or non-managed files
        }

        return null;
    }

    public static int GetFrameworkFamilyRank(NuGetFramework framework)
    {
        string identifier = framework.Framework;

        return identifier switch
        {
            FrameworkIdentifiers.NetCoreApp or FrameworkIdentifiers.NetCore or FrameworkIdentifiers.DotNet => 1,
            FrameworkIdentifiers.NetStandard or FrameworkIdentifiers.NetStandardApp => 2,
            FrameworkIdentifiers.Net => 3,
            _ => 4
        };
    }
}