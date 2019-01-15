using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TypeGen.Cli.Extensions;
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
        
        [DataMember(Name = "generationSpecs")]
        public string[] GenerationSpecs { get; set; }

        [DataMember(Name = "fileNameConverters")]
        public string[] FileNameConverters { get; set; }

        [DataMember(Name = "typeNameConverters")]
        public string[] TypeNameConverters { get; set; }

        [DataMember(Name = "propertyNameConverters")]
        public string[] PropertyNameConverters { get; set; }

        [DataMember(Name = "enumValueNameConverters")]
        public string[] EnumValueNameConverters { get; set; }
        
        [DataMember(Name = "enumStringInitializersConverters")]
        public string[] EnumStringInitializersConverters { get; set; }

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
        
        [DataMember(Name = "clearOutputDirectory")]
        public bool? ClearOutputDirectory { get; set; }

        [DataMember(Name = "createIndexFile")]
        public bool? CreateIndexFile { get; set; }

        [DataMember(Name = "strictNullChecks")]
        public bool? StrictNullChecks { get; set; }

        [DataMember(Name = "csNullableTranslation")]
        public string CsNullableTranslation { get; set; }
        
        [DataMember(Name = "defaultValuesForTypes")]
        public Dictionary<string, string> DefaultValuesForTypes { get; set; }
        
        [DataMember(Name = "customTypeMappings")]
        public Dictionary<string, string> CustomTypeMappings { get; set; }
        
        [DataMember(Name = "generateFromAssemblies")]
        public bool? GenerateFromAssemblies { get; set; }
        
        [DataMember(Name = "useAttributesWithGenerationSpec")]
        public bool? UseAttributesWithGenerationSpec { get; set; }
        
        [DataMember(Name = "enumStringInitializers")]
        public bool? EnumStringInitializers { get; set; }
        
        [DataMember(Name = "fileHeading")]
        public string FileHeading { get; set; }

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
            if (AddFilesToProject == null) AddFilesToProject = DefaultAddFilesToProject;
            if (TypeScriptFileExtension == null) TypeScriptFileExtension = GeneratorOptions.DefaultTypeScriptFileExtension;
            if (TabLength == null) TabLength = GeneratorOptions.DefaultTabLength;
            if (FileNameConverters == null) FileNameConverters = GeneratorOptions.DefaultFileNameConverters.GetTypeNames().ToArray();
            if (TypeNameConverters == null) TypeNameConverters = GeneratorOptions.DefaultTypeNameConverters.GetTypeNames().ToArray();
            if (PropertyNameConverters == null) PropertyNameConverters = GeneratorOptions.DefaultPropertyNameConverters.GetTypeNames().ToArray();
            if (EnumValueNameConverters == null) EnumValueNameConverters = GeneratorOptions.DefaultEnumValueNameConverters.GetTypeNames().ToArray();
            if (EnumStringInitializersConverters == null) EnumStringInitializersConverters = GeneratorOptions.DefaultEnumStringInitializersConverters.GetTypeNames().ToArray();
            if (ExternalAssemblyPaths == null) ExternalAssemblyPaths = new string[0];
            if (CreateIndexFile == null) CreateIndexFile = GeneratorOptions.DefaultCreateIndexFile;
            if (StrictNullChecks == null) StrictNullChecks = GeneratorOptions.DefaultStrictNullChecks;
            if (CsNullableTranslation == null) CsNullableTranslation = GeneratorOptions.DefaultCsNullableTranslation.ToFlagString();
            if (OutputPath == null) OutputPath = "";
            if (ClearOutputDirectory == null) ClearOutputDirectory = false;
            if (DefaultValuesForTypes == null) DefaultValuesForTypes = GeneratorOptions.DefaultDefaultValuesForTypes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            if (CustomTypeMappings == null) CustomTypeMappings = GeneratorOptions.DefaultCustomTypeMappings.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            // GenerateFromAssemblies should stay null if no value is provided
            if (UseAttributesWithGenerationSpec == null) UseAttributesWithGenerationSpec = GeneratorOptions.DefaultUseAttributesWithGenerationSpec;
            if (EnumStringInitializers == null) EnumStringInitializers = GeneratorOptions.DefaultEnumStringInitializers;
            if (FileHeading == null) FileHeading = GeneratorOptions.DefaultFileHeading;
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
