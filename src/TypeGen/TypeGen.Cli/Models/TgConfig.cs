using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TypeGen.Cli.Extensions;
using TypeGen.Core;
using TypeGen.Core.Converters;
using TypeGen.Core.Extensions;
using TypeGen.Core.Generator;

namespace TypeGen.Cli.Models
{
    /// <summary>
    /// Represents console configuration
    /// </summary>
    internal class TgConfig
    {
        public static bool DefaultAddFilesToProject => false;
        public static bool DefaultBuildProject => false;

        [Obsolete("Use Assemblies instead")]
        public string AssemblyPath { get; set; }
        public string[] Assemblies { get; set; }
        public string[] GenerationSpecs { get; set; }
        public string[] FileNameConverters { get; set; }
        public string[] TypeNameConverters { get; set; }
        public string[] PropertyNameConverters { get; set; }
        public string[] EnumValueNameConverters { get; set; }
        public string[] EnumStringInitializersConverters { get; set; }
        public string[] ExternalAssemblyPaths { get; set; }
        public string TypeScriptFileExtension { get; set; }
        public int? TabLength { get; set; }
        public bool? UseTabCharacter { get; set; }
        public bool? ExplicitPublicAccessor { get; set; }
        public bool? SingleQuotes { get; set; }
        public bool? BuildProject { get; set; }
        public bool? AddFilesToProject { get; set; }
        public string OutputPath { get; set; }
        public bool? ClearOutputDirectory { get; set; }
        public bool? CreateIndexFile { get; set; }
        public string CsNullableTranslation { get; set; }
        public bool? CsAllowNullsForAllTypes { get; set; }
        public Dictionary<string, string> DefaultValuesForTypes { get; set; }
        public Dictionary<string, IEnumerable<string>> TypeUnionsForTypes { get; set; }
        public Dictionary<string, string> CustomTypeMappings { get; set; }
        public bool? GenerateFromAssemblies { get; set; }
        public bool? EnumStringInitializers { get; set; }
        public string FileHeading { get; set; }
        public bool? UseDefaultExport { get; set; }

        public TgConfig Normalize()
        {
            if (ExternalAssemblyPaths.Contains("<global-packages>"))
            {
                List<string> newExternalAssemblyPaths = ExternalAssemblyPaths.ToList();
                newExternalAssemblyPaths.Remove("<global-packages>");
                ExternalAssemblyPaths = newExternalAssemblyPaths.ToArray();
            }

            return this;
        }

        public TgConfig MergeWithDefaultParams()
        {
            if (Assemblies == null) Assemblies = new string[0];
            if (GenerationSpecs == null) GenerationSpecs = new string[0];
            if (ExplicitPublicAccessor == null) ExplicitPublicAccessor = GeneratorOptions.DefaultExplicitPublicAccessor;
            if (SingleQuotes == null) SingleQuotes = GeneratorOptions.DefaultSingleQuotes;
            if (BuildProject == null) BuildProject = DefaultBuildProject;
            if (AddFilesToProject == null) AddFilesToProject = DefaultAddFilesToProject;
            if (TypeScriptFileExtension == null) TypeScriptFileExtension = GeneratorOptions.DefaultTypeScriptFileExtension;
            if (TabLength == null) TabLength = GeneratorOptions.DefaultTabLength;
            if (UseTabCharacter == null) UseTabCharacter = GeneratorOptions.DefaultUseTabCharacter;
            if (FileNameConverters == null) FileNameConverters = GeneratorOptions.DefaultFileNameConverters.GetTypeNames().ToArray();
            if (TypeNameConverters == null) TypeNameConverters = GeneratorOptions.DefaultTypeNameConverters.GetTypeNames().ToArray();
            if (PropertyNameConverters == null) PropertyNameConverters = GeneratorOptions.DefaultPropertyNameConverters.GetTypeNames().ToArray();
            if (EnumValueNameConverters == null) EnumValueNameConverters = GeneratorOptions.DefaultEnumValueNameConverters.GetTypeNames().ToArray();
            if (EnumStringInitializersConverters == null) EnumStringInitializersConverters = GeneratorOptions.DefaultEnumStringInitializersConverters.GetTypeNames().ToArray();
            if (ExternalAssemblyPaths == null) ExternalAssemblyPaths = new string[0];
            if (CreateIndexFile == null) CreateIndexFile = GeneratorOptions.DefaultCreateIndexFile;
            if (CsNullableTranslation == null) CsNullableTranslation = GeneratorOptions.DefaultCsNullableTranslation.ToFlagString();
            if (CsAllowNullsForAllTypes == null) CsAllowNullsForAllTypes = GeneratorOptions.DefaultCsAllowNullsForAllTypes;
            if (OutputPath == null) OutputPath = "";
            if (ClearOutputDirectory == null) ClearOutputDirectory = false;
            if (DefaultValuesForTypes == null) DefaultValuesForTypes = GeneratorOptions.DefaultDefaultValuesForTypes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            if (TypeUnionsForTypes == null) TypeUnionsForTypes = GeneratorOptions.DefaultTypeUnionsForTypes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            if (CustomTypeMappings == null) CustomTypeMappings = GeneratorOptions.DefaultCustomTypeMappings.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            // GenerateFromAssemblies should stay null if no value is provided
            if (EnumStringInitializers == null) EnumStringInitializers = GeneratorOptions.DefaultEnumStringInitializers;
            if (FileHeading == null) FileHeading = GeneratorOptions.DefaultFileHeading;
            if (UseDefaultExport == null) UseDefaultExport = GeneratorOptions.DefaultUseDefaultExport;
            return this;
        }

        public string[] GetAssemblies()
        {
            return Assemblies.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(AssemblyPath) ?
                new[] { AssemblyPath } :
                Assemblies;
        }
    }
}
