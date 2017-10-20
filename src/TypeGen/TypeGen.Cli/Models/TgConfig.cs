using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TypeGen.Core;
using TypeGen.Core.Converters;
using TypeGen.Core.Extensions;

namespace TypeGen.Cli.Models
{
    /// <summary>
    /// Represents console configuration
    /// </summary>
    [DataContract]
    internal class TgConfig
    {
        public static bool DefaultAddFilesToProject => false;

        [Obsolete("Use Assemblies instead")]
        [DataMember(Name = "assemblyPath")]
        public string AssemblyPath { get; set; }

        [DataMember(Name = "assemblies")]
        public string[] Assemblies { get; set; }

        [DataMember(Name = "fileNameConverters")]
        public string[] FileNameConverters { get; set; }

        [DataMember(Name = "typeNameConverters")]
        public string[] TypeNameConverters { get; set; }

        [DataMember(Name = "propertyNameConverters")]
        public string[] PropertyNameConverters { get; set; }

        [DataMember(Name = "enumValueNameConverters")]
        public string[] EnumValueNameConverters { get; set; }

        [DataMember(Name = "externalAssemblyPaths")]
        public string[] ExternalAssemblyPaths { get; set; }

        [DataMember(Name = "typeScriptFileExtension")]
        public string TypeScriptFileExtension { get; set; }

        [DataMember(Name = "tabLength")]
        public int? TabLength { get; set; }

        [DataMember(Name = "explicitPublicAccessor")]
        public bool? ExplicitPublicAccessor { get; set; }

        [DataMember(Name = "singleQuotes")]
        public bool? SingleQuotes { get; set; }

        [DataMember(Name = "addFilesToProject")]
        public bool? AddFilesToProject { get; set; }

        [DataMember(Name = "outputPath")]
        public string OutputPath { get; set; }

        public TgConfig Normalize()
        {
            AssemblyPath = AssemblyPath.NormalizePath();
            Assemblies = Assemblies.Select(FileSystemExtensions.NormalizePath).ToArray();
            FileNameConverters = FileNameConverters.Select(FileSystemExtensions.NormalizePath).ToArray();
            TypeNameConverters = TypeNameConverters.Select(FileSystemExtensions.NormalizePath).ToArray();
            PropertyNameConverters = PropertyNameConverters.Select(FileSystemExtensions.NormalizePath).ToArray();
            EnumValueNameConverters = EnumValueNameConverters.Select(FileSystemExtensions.NormalizePath).ToArray();
            ExternalAssemblyPaths = ExternalAssemblyPaths.Select(FileSystemExtensions.NormalizePath).ToArray();
            OutputPath = OutputPath.NormalizePath();

            ReplaceAllTags();

            return this;
        }

        private void ReplaceAllTags()
        {
            AssemblyPath = ReplacePathTags(AssemblyPath);
            ExternalAssemblyPaths = ExternalAssemblyPaths.Select(ReplacePathTags).ToArray();
            OutputPath = ReplacePathTags(OutputPath);
        }

        public TgConfig MergeWithDefaultParams()
        {
            if (Assemblies == null) Assemblies = new string[0];
            if (ExplicitPublicAccessor == null) ExplicitPublicAccessor = GeneratorOptions.DefaultExplicitPublicAccessor;
            if (SingleQuotes == null) SingleQuotes = GeneratorOptions.DefaultSingleQuotes;
            if (AddFilesToProject == null) AddFilesToProject = DefaultAddFilesToProject;
            if (TypeScriptFileExtension == null) TypeScriptFileExtension = GeneratorOptions.DefaultTypeScriptFileExtension;
            if (TabLength == null) TabLength = GeneratorOptions.DefaultTabLength;
            if (FileNameConverters == null) FileNameConverters = GeneratorOptions.DefaultFileNameConverters.GetTypeNames().ToArray();
            if (TypeNameConverters == null) TypeNameConverters = GeneratorOptions.DefaultTypeNameConverters.GetTypeNames().ToArray();
            if (PropertyNameConverters == null) PropertyNameConverters = GeneratorOptions.DefaultPropertyNameConverters.GetTypeNames().ToArray();
            if (EnumValueNameConverters == null) EnumValueNameConverters = GeneratorOptions.DefaultEnumValueNameConverters.GetTypeNames().ToArray();
            if (ExternalAssemblyPaths == null) ExternalAssemblyPaths = new string[0];
            return this;
        }

        public string[] GetAssemblies()
        {
            return Assemblies.IsNullOrEmpty() ?
                new[] { AssemblyPath } :
                Assemblies;
        }

        private static string GetTag(string name)
        {
            return $"<{name}>";
        }

        private static string ReplacePathTags(string path)
        {
            return path?.Replace(GetTag("global-packages"), CliSettings.GlobalPackagesPath);
        }
    }
}
