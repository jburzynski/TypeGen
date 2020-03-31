using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TypeGen.Core.Extensions;
using TypeGen.Core.Generator.Services;
using TypeGen.Core.Logging;
using TypeGen.Core.Metadata;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.Storage;
using TypeGen.Core.TypeAnnotations;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Generator
{
    /// <summary>
    /// Class used for generating TypeScript files from C# types
    /// </summary>
    public class Generator
    {
        /// <summary>
        /// An event that fires when a file's content is generated
        /// </summary>
        public event EventHandler<FileContentGeneratedArgs> FileContentGenerated;
        
        /// <summary>
        /// A logger instance used to log messages raised by a Generator instance
        /// </summary>
        public ILogger Logger { get; }
        
        /// <summary>
        /// Generator options. Cannot be null.
        /// </summary>
        public GeneratorOptions Options { get; }
        
        private readonly MetadataReaderFactory _metadataReaderFactory;
        private readonly ITypeService _typeService;
        private readonly ITypeDependencyService _typeDependencyService;
        private readonly ITemplateService _templateService;
        private readonly ITsContentGenerator _tsContentGenerator;
        private readonly IFileSystem _fileSystem;

        // keeps track of what types have been generated in the current session
        private readonly GenerationContext _generationContext;

        public Generator(GeneratorOptions options, ILogger logger = null)
        {
            Requires.NotNull(options, nameof(options));
            
            _generationContext = new GenerationContext();
            FileContentGenerated += OnFileContentGenerated;
            
            Options = options;
            Logger = logger;
            
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = options };

            var internalStorage = new InternalStorage();
            _fileSystem = new FileSystem();
            _metadataReaderFactory = new MetadataReaderFactory();
            _typeService = new TypeService(_metadataReaderFactory, generatorOptionsProvider);
            _typeDependencyService = new TypeDependencyService(_typeService, _metadataReaderFactory);
            _templateService = new TemplateService(internalStorage, generatorOptionsProvider);

            _tsContentGenerator = new TsContentGenerator(_typeDependencyService,
                _typeService,
                _templateService,
                new TsContentParser(_fileSystem),
                _metadataReaderFactory,
                generatorOptionsProvider,
                logger);
        }
        
        public Generator(ILogger logger) : this(new GeneratorOptions(), logger)
        {
        }

        public Generator() : this(new GeneratorOptions())
        {
        }

        /// <summary>
        /// For unit testing (mocking FileSystem)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fileSystem"></param>
        internal Generator(GeneratorOptions options, IFileSystem fileSystem) : this(options)
        {
            _fileSystem = fileSystem;
        }
        
        /// <summary>
        /// The default event handler for the FileContentGenerated event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void OnFileContentGenerated(object sender, FileContentGeneratedArgs args)
        {
            _fileSystem.SaveFile(args.FilePath, args.FileContent);
        }

        /// <summary>
        /// Subscribes the default FileContentGenerated event handler, which saves generated sources to the file system
        /// </summary>
        public void SubscribeDefaultFileContentGeneratedHandler()
        {
            FileContentGenerated -= OnFileContentGenerated;
            FileContentGenerated += OnFileContentGenerated;
        }
        
        /// <summary>
        /// Unsubscribes the default FileContentGenerated event handler, which saves generated sources to the file system
        /// </summary>
        public void UnsubscribeDefaultFileContentGeneratedHandler()
        {
            FileContentGenerated -= OnFileContentGenerated;
        }

        private void InitializeGeneration(GenerationSpec generationSpec)
        {
            _metadataReaderFactory.GenerationSpec = generationSpec;
        }

        /// <summary>
        /// Generates TypeScript files from a GenerationSpec
        /// </summary>
        /// <param name="generationSpecs"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        public Task<IEnumerable<string>> GenerateAsync(IEnumerable<GenerationSpec> generationSpecs)
        {
            return Task.Run(() => Generate(generationSpecs));
        }

        /// <summary>
        /// Generates TypeScript files from a GenerationSpec
        /// </summary>
        /// <param name="generationSpecs"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        public IEnumerable<string> Generate(IEnumerable<GenerationSpec> generationSpecs)
        {
            Requires.NotNullOrEmpty(generationSpecs, nameof(generationSpecs));
            
            var files = new List<string>();
            
            // generate types
            
            _generationContext.InitializeGroupGeneratedTypes();

            foreach (GenerationSpec generationSpec in generationSpecs)
            {
                InitializeGeneration(generationSpec);
                generationSpec.OnBeforeGeneration(new OnBeforeGenerationArgs(Options));

                foreach (KeyValuePair<Type, TypeSpec> kvp in generationSpec.TypeSpecs)
                {
                    files.AddRange(GenerateTypeInit(kvp.Key));
                }
            }
            
            files = files.Distinct().ToList();
            
            _generationContext.ClearGroupGeneratedTypes();
            
            // generate barrels
            
            if (Options.CreateIndexFile)
            {
                files.AddRange(GenerateIndexFile(files));
            }
            
            foreach (GenerationSpec generationSpec in generationSpecs)
            {
                generationSpec.OnBeforeBarrelGeneration(new OnBeforeBarrelGenerationArgs(Options, files));
            }
            
            foreach (GenerationSpec generationSpec in generationSpecs)
            {
                foreach (BarrelSpec barrelSpec in generationSpec.BarrelSpecs)
                {
                    files.AddRange(GenerateBarrel(barrelSpec));
                }
            }

            return files;
        }

        private IEnumerable<string> GenerateBarrel(BarrelSpec barrelSpec)
        {
            string directory = Path.Combine(Options.BaseOutputDirectory?.EnsurePostfix("/") ?? "", barrelSpec.Directory);
            
            var fileName = "index";
            if (!string.IsNullOrWhiteSpace(Options.TypeScriptFileExtension)) fileName += $".{Options.TypeScriptFileExtension}";
            string filePath = Path.Combine(directory.EnsurePostfix("/"), fileName);

            var entries = new List<string>();
            
            if (barrelSpec.BarrelScope.HasFlag(BarrelScope.Files))
            {
                entries.AddRange(_fileSystem.GetDirectoryFiles(directory)
                    .Where(x => Path.GetFileName(x) != fileName && x.EndsWith($".{Options.TypeScriptFileExtension}"))
                    .Select(Path.GetFileNameWithoutExtension));
            }
            
            if (barrelSpec.BarrelScope.HasFlag(BarrelScope.Directories))
            {
                entries.AddRange(
                    _fileSystem.GetDirectoryDirectories(directory)
                        .Select(dir => dir.Replace("\\", "/").Split('/').Last())
                    );
            }

            string indexExportsContent = entries.Aggregate("", (acc, entry) => acc += _templateService.FillIndexExportTemplate(entry));
            string content = _templateService.FillIndexTemplate(indexExportsContent);
            
            FileContentGenerated?.Invoke(this, new FileContentGeneratedArgs(null, filePath, content));
            return new[] { Path.Combine(barrelSpec.Directory.EnsurePostfix("/"), fileName) };
        }
        
        /// <summary>
        /// DEPRECATED, will be removed in the future.
        /// Generates an `index.ts` file which exports all types within the generated files
        /// </summary>
        /// <param name="generatedFiles"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        private IEnumerable<string> GenerateIndexFile(IEnumerable<string> generatedFiles)
        {
            var typeScriptFileExtension = "";
            if (!string.IsNullOrEmpty(Options.TypeScriptFileExtension))
            {
                typeScriptFileExtension = "." + Options.TypeScriptFileExtension;
            }

            string exports = generatedFiles.Aggregate("", (prevExports, file) =>
            {
                string fileNameWithoutExt = file.Remove(file.Length - typeScriptFileExtension.Length).Replace("\\", "/");
                return prevExports + _templateService.FillIndexExportTemplate(fileNameWithoutExt);
            });
            string content = _templateService.FillIndexTemplate(exports);

            string filename = "index" + typeScriptFileExtension;
            FileContentGenerated?.Invoke(this, new FileContentGeneratedArgs(null, Path.Combine(Options.BaseOutputDirectory, filename), content));

            return new[] { filename };
        }
        
        private IEnumerable<string> GenerateTypeInit(Type type)
        {
            IEnumerable<string> files = Enumerable.Empty<string>();
            
            _generationContext.InitializeTypeGeneratedTypes();
            _generationContext.Add(type);

            if (_generationContext.IsGroupContext())
            {
                files = GenerateType(type);
            }
            else
            {
                ExecuteWithTypeContextLogging(() => { files = GenerateType(type); });
            }

            _generationContext.ClearTypeGeneratedTypes();

            return files.Distinct();
        }
        
        /// <summary>
        /// Contains the actual logic of generating TypeScript files for a given type
        /// Should only be used inside GenerateTypeInit(), otherwise use GenerateTypeInit()
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        private IEnumerable<string> GenerateType(Type type)
        {
            var classAttribute = _metadataReaderFactory.GetInstance().GetAttribute<ExportTsClassAttribute>(type);
            var interfaceAttribute = _metadataReaderFactory.GetInstance().GetAttribute<ExportTsInterfaceAttribute>(type);
            var enumAttribute = _metadataReaderFactory.GetInstance().GetAttribute<ExportTsEnumAttribute>(type);

            if (classAttribute != null)
            {
                return GenerateClass(type, classAttribute);
            }

            if (interfaceAttribute != null)
            {
                return GenerateInterface(type, interfaceAttribute);
            }

            if (enumAttribute != null)
            {
                return GenerateEnum(type, enumAttribute);
            }

            return GenerateNotMarked(type, Options.BaseOutputDirectory);
        }
        
        /// <summary>
        /// Generates TypeScript files from an assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        public Task<IEnumerable<string>> GenerateAsync(Assembly assembly)
        {
            return Task.Run(() => Generate(assembly));
        }
        
        /// <summary>
        /// Generates TypeScript files from an assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        public IEnumerable<string> Generate(Assembly assembly)
        {
            Requires.NotNull(assembly, nameof(assembly));
            return Generate(new[] { assembly });
        }
        
        /// <summary>
        /// Generates TypeScript files from multiple assemblies
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        public Task<IEnumerable<string>> GenerateAsync(IEnumerable<Assembly> assemblies)
        {
            return Task.Run(() => Generate(assemblies));
        }
        
        /// <summary>
        /// Generates TypeScript files from multiple assemblies
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        public IEnumerable<string> Generate(IEnumerable<Assembly> assemblies)
        {
            Requires.NotNullOrEmpty(assemblies, nameof(assemblies));
            
            var generationSpecProvider = new GenerationSpecProvider();
            GenerationSpec generationSpec = generationSpecProvider.GetGenerationSpec(assemblies);

            return Generate(new[] { generationSpec });
        }

        /// <summary>
        /// Generates TypeScript files from a type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        public Task<IEnumerable<string>> GenerateAsync(Type type)
        {
            return Task.Run(() => Generate(type));
        }
        
        /// <summary>
        /// Generates TypeScript files from a type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        public IEnumerable<string> Generate(Type type)
        {
            Requires.NotNull(type, nameof(type));
            
            var generationSpecProvider = new GenerationSpecProvider();
            GenerationSpec generationSpec = generationSpecProvider.GetGenerationSpec(type);

            return Generate(new[] { generationSpec });
        }

        /// <summary>
        /// Generates TypeScript files for types that are not marked with an ExportTs... attribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDirectory"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        private IEnumerable<string> GenerateNotMarked(Type type, string outputDirectory)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsClass)
            {
                return GenerateClass(type, new ExportTsClassAttribute { OutputDir = outputDirectory });
            }

            if (typeInfo.IsInterface)
            {
                return GenerateInterface(type, new ExportTsInterfaceAttribute { OutputDir = outputDirectory });
            }

            if (typeInfo.IsEnum)
            {
                return GenerateEnum(type, new ExportTsEnumAttribute { OutputDir = outputDirectory });
            }

            throw new CoreException($"Generated type must be either a C# class or enum. Error when generating type {type.FullName}");
        }

        /// <summary>
        /// Generates a TypeScript class file from a class type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="classAttribute"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        private IEnumerable<string> GenerateClass(Type type, ExportTsClassAttribute classAttribute)
        {
            return GenerateClassOrInterface(type, classAttribute, null);
        }

        /// <summary>
        /// Generates a TypeScript interface file from a class type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceAttribute"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        private IEnumerable<string> GenerateInterface(Type type, ExportTsInterfaceAttribute interfaceAttribute)
        {
            return GenerateClassOrInterface(type, null, interfaceAttribute);
        }

        private IEnumerable<string> GenerateClassOrInterface(Type type, ExportTsClassAttribute classAttribute, ExportTsInterfaceAttribute interfaceAttribute)
        {
            string outputDir = classAttribute != null ? classAttribute.OutputDir : interfaceAttribute.OutputDir;
            IEnumerable<string> dependenciesGenerationResult = GenerateTypeDependencies(type, outputDir);

            // get text for sections

            var tsCustomBaseAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsCustomBaseAttribute>(type);
            var extendsText = "";

            if (classAttribute == null)
            {
                // this is an interface, generate extends for an interface.
                extendsText = _tsContentGenerator.GetExtendsForInterfacesText(type);
            }
            else if (tsCustomBaseAttribute != null)
            {
                extendsText = string.IsNullOrEmpty(tsCustomBaseAttribute.Base) ? "" : _templateService.GetExtendsText(tsCustomBaseAttribute.Base);
            }
            else if (_metadataReaderFactory.GetInstance().GetAttribute<TsIgnoreBaseAttribute>(type) == null)
            {
                extendsText = _tsContentGenerator.GetExtendsText(type);
            }

            string implementsText = _tsContentGenerator.GetImplementsText(type);

            string importsText = _tsContentGenerator.GetImportsText(type, outputDir);
            string propertiesText = classAttribute != null ? GetClassPropertiesText(type) : GetInterfacePropertiesText(type);

            // generate the file content

            string tsTypeName = _typeService.GetTsTypeName(type, true);
            string tsTypeNameFirstPart = tsTypeName.RemoveTypeGenericComponent();
            string filePath = GetFilePath(type, outputDir);
            string filePathRelative = GetRelativeFilePath(type, outputDir);
            string customHead = _tsContentGenerator.GetCustomHead(filePath);
            string customBody = _tsContentGenerator.GetCustomBody(filePath, Options.TabLength);

            string content;
            
            if (classAttribute != null)
            {
                content = _typeService.UseDefaultExport(type) ?
                    _templateService.FillClassDefaultExportTemplate(importsText, tsTypeName, tsTypeNameFirstPart, extendsText, implementsText, propertiesText, customHead, customBody, Options.FileHeading) :
                    _templateService.FillClassTemplate(importsText, tsTypeName, extendsText, implementsText, propertiesText, customHead, customBody, Options.FileHeading);
            }
            else
            {
                content = _typeService.UseDefaultExport(type) ?
                    _templateService.FillInterfaceDefaultExportTemplate(importsText, tsTypeName, tsTypeNameFirstPart, extendsText, propertiesText, customHead, customBody, Options.FileHeading) :
                    _templateService.FillInterfaceTemplate(importsText, tsTypeName, extendsText, propertiesText, customHead, customBody, Options.FileHeading);
            }

            // write TypeScript file

            FileContentGenerated?.Invoke(this, new FileContentGeneratedArgs(type, filePath, content));
            return new[] { filePathRelative }.Concat(dependenciesGenerationResult).ToList();
        }

        /// <summary>
        /// Generates a TypeScript enum file from a class type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="enumAttribute"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        private IEnumerable<string> GenerateEnum(Type type, ExportTsEnumAttribute enumAttribute)
        {
            string valuesText = GetEnumMembersText(type);

            // create TypeScript source code for the enum

            string tsEnumName = _typeService.GetTsTypeName(type, true);
            string filePath = GetFilePath(type, enumAttribute.OutputDir);
            string filePathRelative = GetRelativeFilePath(type, enumAttribute.OutputDir);

            string enumText = _typeService.UseDefaultExport(type) ? 
                _templateService.FillEnumDefaultExportTemplate("", tsEnumName, valuesText, enumAttribute.IsConst, Options.FileHeading) :
                _templateService.FillEnumTemplate("", tsEnumName, valuesText, enumAttribute.IsConst, Options.FileHeading);

            // write TypeScript file

            FileContentGenerated?.Invoke(this, new FileContentGeneratedArgs(type, filePath, enumText));
            return new[] { filePathRelative };
        }

        private bool IsStaticTsProperty(MemberInfo memberInfo)
        {
            if (_metadataReaderFactory.GetInstance().GetAttribute<TsNotStaticAttribute>(memberInfo) != null) return false;
            return _metadataReaderFactory.GetInstance().GetAttribute<TsStaticAttribute>(memberInfo) != null || memberInfo.IsStatic();
        }
        
        private bool IsReadonlyTsProperty(MemberInfo memberInfo)
        {
            if (_metadataReaderFactory.GetInstance().GetAttribute<TsNotReadonlyAttribute>(memberInfo) != null) return false;
            return _metadataReaderFactory.GetInstance().GetAttribute<TsReadonlyAttribute>(memberInfo) != null || (memberInfo is FieldInfo fi && (fi.IsInitOnly || fi.IsLiteral));
        }

        /// <summary>
        /// Gets TypeScript class property definition source code
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        private string GetClassPropertyText(MemberInfo memberInfo)
        {
            LogClassPropertyWarnings(memberInfo);
            
            string modifiers = Options.ExplicitPublicAccessor ? "public " : "";

            if (IsStaticTsProperty(memberInfo)) modifiers += "static ";
            if (IsReadonlyTsProperty(memberInfo)) modifiers += "readonly ";

            var nameAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsMemberNameAttribute>(memberInfo);
            string name = nameAttribute?.Name ?? Options.PropertyNameConverters.Convert(memberInfo.Name, memberInfo);
            string typeName = _typeService.GetTsTypeName(memberInfo);
            IEnumerable<string> typeUnions = _typeService.GetTypeUnions(memberInfo);

            // try to get default value from TsDefaultValueAttribute
            var defaultValueAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsDefaultValueAttribute>(memberInfo);
            if (defaultValueAttribute != null)
                return _templateService.FillClassPropertyTemplate(modifiers, name, typeName, typeUnions, defaultValueAttribute.DefaultValue);

            // try to get default value from the member's default value
            string valueText = _tsContentGenerator.GetMemberValueText(memberInfo);
            if (!string.IsNullOrWhiteSpace(valueText))
                return _templateService.FillClassPropertyTemplate(modifiers, name, typeName, typeUnions, valueText);

            // try to get default value from Options.DefaultValuesForTypes
            if (Options.DefaultValuesForTypes.Any() && Options.DefaultValuesForTypes.ContainsKey(typeName))
                return _templateService.FillClassPropertyTemplate(modifiers, name, typeName, typeUnions, Options.DefaultValuesForTypes[typeName]);

            return _templateService.FillClassPropertyTemplate(modifiers, name, typeName, typeUnions);
        }
        
        private void LogClassPropertyWarnings(MemberInfo memberInfo)
        {
            if (Logger == null) return;
            
            if (_metadataReaderFactory.GetInstance().GetAttribute<TsOptionalAttribute>(memberInfo) != null)
                Logger.Log($"TsOptionalAttribute used for a class property ({memberInfo.DeclaringType?.FullName}.{memberInfo.Name}). The attribute will be ignored.", LogLevel.Warning);
        }

        /// <summary>
        /// Gets TypeScript class properties definition source code
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetClassPropertiesText(Type type)
        {
            var propertiesText = "";
            IEnumerable<MemberInfo> memberInfos = type.GetTsExportableMembers(_metadataReaderFactory.GetInstance());

            // create TypeScript source code for properties' definition

            propertiesText += memberInfos
                .Aggregate(propertiesText, (current, memberInfo) => current + GetClassPropertyText(memberInfo));

            return RemoveLastLineEnding(propertiesText);
        }

        /// <summary>
        /// Gets TypeScript interface property definition source code
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        private string GetInterfacePropertyText(MemberInfo memberInfo)
        {
            LogInterfacePropertyWarnings(memberInfo);
            
            string modifiers = "";
            if (IsReadonlyTsProperty(memberInfo)) modifiers += "readonly ";
            
            var nameAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsMemberNameAttribute>(memberInfo);
            string name = nameAttribute?.Name ?? Options.PropertyNameConverters.Convert(memberInfo.Name, memberInfo);

            string typeName = _typeService.GetTsTypeName(memberInfo);
            IEnumerable<string> typeUnions = _typeService.GetTypeUnions(memberInfo);
            bool isOptional = _metadataReaderFactory.GetInstance().GetAttribute<TsOptionalAttribute>(memberInfo) != null;

            return _templateService.FillInterfacePropertyTemplate(modifiers, name, typeName, typeUnions, isOptional);
        }

        private void LogInterfacePropertyWarnings(MemberInfo memberInfo)
        {
            if (Logger == null) return;
            
            if (_metadataReaderFactory.GetInstance().GetAttribute<TsStaticAttribute>(memberInfo) != null)
                Logger.Log($"TsStaticAttribute used for an interface property ({memberInfo.DeclaringType?.FullName}.{memberInfo.Name}). The attribute will be ignored.", LogLevel.Warning);
            
            if (_metadataReaderFactory.GetInstance().GetAttribute<TsNotStaticAttribute>(memberInfo) != null)
                Logger.Log($"TsNotStaticAttribute used for an interface property ({memberInfo.DeclaringType?.FullName}.{memberInfo.Name}). The attribute will be ignored.", LogLevel.Warning);
            
            if (_metadataReaderFactory.GetInstance().GetAttribute<TsDefaultValueAttribute>(memberInfo) != null)
                Logger.Log($"TsDefaultValueAttribute used for an interface property ({memberInfo.DeclaringType?.FullName}.{memberInfo.Name}). The attribute will be ignored.", LogLevel.Warning);
        }

        /// <summary>
        /// Gets TypeScript interface properties definition source code
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetInterfacePropertiesText(Type type)
        {
            var propertiesText = "";
            IEnumerable<MemberInfo> memberInfos = type.GetTsExportableMembers(_metadataReaderFactory.GetInstance());

            // create TypeScript source code for properties' definition

            propertiesText += memberInfos
                .Aggregate(propertiesText, (current, memberInfo) => current + GetInterfacePropertyText(memberInfo));

            return RemoveLastLineEnding(propertiesText);
        }

        /// <summary>
        /// Gets TypeScript enum member definition source code
        /// </summary>
        /// <param name="fieldInfo">MemberInfo for an enum value</param>
        /// <returns></returns>
        private string GetEnumMemberText(FieldInfo fieldInfo)
        {
            Type type = fieldInfo.DeclaringType;
            
            string name = Options.EnumValueNameConverters.Convert(fieldInfo.Name, fieldInfo);
            var stringInitializersAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsStringInitializersAttribute>(type);
            
            if ((Options.EnumStringInitializers && (stringInitializersAttribute == null || stringInitializersAttribute.Enabled)) ||
                (stringInitializersAttribute != null && stringInitializersAttribute.Enabled))
            {
                string enumValueString = Options.EnumStringInitializersConverters.Convert(fieldInfo.Name, fieldInfo);
                return _templateService.FillEnumValueTemplate(name, enumValueString);
            }

            object enumValue = fieldInfo.GetValue(null);
            object enumValueAsUnderlyingType = Convert.ChangeType(enumValue, Enum.GetUnderlyingType(type));
            return _templateService.FillEnumValueTemplate(name, enumValueAsUnderlyingType);
        }

        /// <summary>
        /// Gets TypeScript enum member definition source code
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetEnumMembersText(Type type)
        {
            var valuesText = "";
            IEnumerable<FieldInfo> fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            valuesText += fieldInfos.Aggregate(valuesText, (current, fieldInfo) => current + GetEnumMemberText(fieldInfo));

            return RemoveLastLineEnding(valuesText);
        }

        /// <summary>
        /// Generates type dependencies' files for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        private IEnumerable<string> GenerateTypeDependencies(Type type, string outputDir)
        {
            var generatedFiles = new List<string>();
            IEnumerable<TypeDependencyInfo> typeDependencies = _typeDependencyService.GetTypeDependencies(type);

            foreach (TypeDependencyInfo typeDependencyInfo in typeDependencies)
            {
                Type typeDependency = typeDependencyInfo.Type;

                // dependency type TypeScript file generation

                // dependency HAS an ExportTsX attribute (AND hasn't been generated yet)
                if (typeDependency.HasExportAttribute(_metadataReaderFactory.GetInstance()) && !_generationContext.HasBeenGeneratedForGroup(typeDependency))
                {
                    _generationContext.Add(typeDependency);
                    generatedFiles.AddRange(GenerateTypeInit(typeDependency));
                }

                // dependency DOESN'T HAVE an ExportTsX attribute (AND hasn't been generated for the currently generated type yet)
                if (!typeDependency.HasExportAttribute(_metadataReaderFactory.GetInstance()) && !_generationContext.HasBeenGeneratedForType(typeDependency))
                {
                    var defaultOutputAttribute = typeDependencyInfo.MemberAttributes
                        ?.FirstOrDefault(a => a is TsDefaultTypeOutputAttribute)
                        as TsDefaultTypeOutputAttribute;

                    string defaultOutputDir = defaultOutputAttribute?.OutputDir ?? outputDir;

                    _generationContext.Add(typeDependency);
                    generatedFiles.AddRange(GenerateNotMarked(typeDependency, defaultOutputDir));
                }
            }

            return generatedFiles;
        }

        /// <summary>
        /// Gets the output TypeScript file path based on a type.
        /// The path is relative to the base output directory.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        private string GetRelativeFilePath(Type type, string outputDir)
        {
            string typeName = type.Name.RemoveTypeArity();
            string fileName = Options.FileNameConverters.Convert(typeName, type);

            if (!string.IsNullOrEmpty(Options.TypeScriptFileExtension))
            {
                fileName += $".{Options.TypeScriptFileExtension}";
            }

            return string.IsNullOrEmpty(outputDir)
                ? fileName
                : Path.Combine(outputDir.EnsurePostfix("/"), fileName);
        }

        /// <summary>
        /// Gets the output TypeScript file path based on a type.
        /// The path includes base output directory.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        private string GetFilePath(Type type, string outputDir)
        {
            string fileName = GetRelativeFilePath(type, outputDir);
            return Path.Combine(Options.BaseOutputDirectory?.EnsurePostfix("/") ?? "", fileName);
        }

        /// <summary>
        /// Executes the passed action and adds additional info about the currently generated types in case of a CoreException
        /// </summary>
        /// <param name="action"></param>
        private void ExecuteWithTypeContextLogging(Action action)
        {
            try
            {
                action();
            }
            catch (CoreException e)
            {
                if (_generationContext.TypeGeneratedTypes != null)
                {
                    throw new CoreException(e.Message + "; inside type: " +
                                            string.Join(", in ", _generationContext.TypeGeneratedTypes.Reverse().Select(t => t.FullName)));
                }

                throw;
            }
        }

        private static string RemoveLastLineEnding(string propertiesText)
        {
            return propertiesText.TrimEnd('\r', '\n');
        }
    }
}
