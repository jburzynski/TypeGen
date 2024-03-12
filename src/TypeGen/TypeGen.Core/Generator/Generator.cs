﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TypeGen.Core.Conversion;
using TypeGen.Core.Extensions;
using TypeGen.Core.Generator.Context;
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
        private ILogger Logger { get; set; }

        /// <summary>
        /// Generator options. Cannot be null.
        /// </summary>
        public GeneratorOptions Options { get; set; }

        private IMetadataReaderFactory _metadataReaderFactory;
        private ITypeService _typeService;
        private ITypeDependencyService _typeDependencyService;
        private ITemplateService _templateService;
        private ITsContentGenerator _tsContentGenerator;
        private IFileSystem _fileSystem;

        // keeps track of what types have been generated in the current session
        private GenerationContext _generationContext;

        [ActivatorUtilitiesConstructor]
        public Generator(
            IOptions<GeneratorOptions> options, 
            ILogger logger,
            IFileSystem fileSystem,
            IMetadataReaderFactory metadataReaderFactory,
            ITypeService typeService,
            ITypeDependencyService typeDependencyService,
            ITemplateService templateService,
            ITsContentGenerator tsContentGenerator
        ) => Init(options, logger, fileSystem, metadataReaderFactory, typeService, typeDependencyService, templateService, tsContentGenerator);

        protected Generator(GeneratorOptions generatorOptions, ILogger logger = null)
        {
            var options = new OptionsWrapper<GeneratorOptions>(generatorOptions);
            var internalStorage = new InternalStorage();
            var fileSystem = new FileSystem();
            var metadataReaderFactory = new MetadataReaderFactory();
            var typeService = new TypeService(metadataReaderFactory, options);
            var typeDependencyService = new TypeDependencyService(typeService, metadataReaderFactory, options);
            var templateService = new TemplateService(internalStorage, options);

            var tsContentGenerator = new TsContentGenerator(typeDependencyService,
                typeService,
                templateService,
                new TsContentParser(fileSystem),
                metadataReaderFactory,
                options,
                logger);
            
            Init(options, logger, fileSystem, metadataReaderFactory, typeService, typeDependencyService, templateService, tsContentGenerator);
        }

        protected Generator(ILogger logger) : this(new GeneratorOptions(), logger)
        {
        }

        protected Generator() : this(new GeneratorOptions())
        {
        }

        public static Generator Get(GeneratorOptions generatorOptions, ILogger logger = null)
        {
            return new Generator(generatorOptions, logger);
        }

        public static Generator Get()
        {
            return new Generator();
        }

        private void Init(
            IOptions<GeneratorOptions> options, 
            ILogger logger,
            IFileSystem fileSystem,
            IMetadataReaderFactory metadataReaderFactory,
            ITypeService typeService,
            ITypeDependencyService typeDependencyService,
            ITemplateService templateService,
            ITsContentGenerator tsContentGenerator
        )
        {
            Requires.NotNull(options, nameof(options));
            
            FileContentGenerated += OnFileContentGenerated;
            
            Options = options.Value;
            Logger = logger;

            _fileSystem = fileSystem;
            _metadataReaderFactory = metadataReaderFactory;
            _typeService = typeService;
            _typeDependencyService = typeDependencyService;
            _templateService = templateService;
            _tsContentGenerator = tsContentGenerator;
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
        public Task<IEnumerable<string>> GenerateAsync(params GenerationSpec[] generationSpecs)
        {
            return GenerateAsync((IEnumerable<GenerationSpec>)generationSpecs);
        }

        /// <summary>
        /// Generates TypeScript sources from GenerationSpecs.
        /// </summary>
        /// <param name="generationSpecs"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        public IEnumerable<string> Generate(params GenerationSpec[] generationSpecs)
        {
            return Generate((IEnumerable<GenerationSpec>)generationSpecs);
        }

        /// <summary>
        /// Generates TypeScript sources from GenerationSpecs.
        /// </summary>
        /// <param name="generationSpecs"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        public IEnumerable<string> Generate(IEnumerable<GenerationSpec> generationSpecs)
        {
            Requires.NotNullOrEmpty(generationSpecs, nameof(generationSpecs));

            generationSpecs = generationSpecs.ToList();
            var files = new List<string>();
            _generationContext = new GenerationContext(_fileSystem);
            
            // generate types

            foreach (GenerationSpec generationSpec in generationSpecs)
                generationSpec.OnBeforeGeneration(new OnBeforeGenerationArgs(Options));
            
            foreach (GenerationSpec generationSpec in generationSpecs)
            {
                _metadataReaderFactory.GenerationSpec = generationSpec;
                
                foreach (KeyValuePair<Type, TypeSpec> kvp in generationSpec.TypeSpecs)
                    files.AddRange(GenerateMarkedType(kvp.Key));
            }
            
            files = files.Distinct().ToList();
            
            // generate barrels
            
            foreach (GenerationSpec generationSpec in generationSpecs)
                generationSpec.OnBeforeBarrelGeneration(new OnBeforeBarrelGenerationArgs(Options, files.ToList()));
            
            if (Options.CreateIndexFile)
                files.AddRange(GenerateIndexFile(files));
            
            foreach (GenerationSpec generationSpec in generationSpecs)
                foreach (BarrelSpec barrelSpec in generationSpec.BarrelSpecs)
                    files.AddRange(GenerateBarrel(barrelSpec));
            
            foreach (GenerationSpec generationSpec in generationSpecs)
                generationSpec.OnAfterGeneration(new OnAfterGenerationArgs(Options, files.ToList()));

            return files;
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
        /// DEPRECATED, can be removed in the future.
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
        
        private IEnumerable<string> GenerateMarkedType(Type type)
        {
            if (Options.IsTypeBlacklisted(type)) return Enumerable.Empty<string>();
            
            IEnumerable<string> files = Enumerable.Empty<string>();
            
            _generationContext.BeginTypeGeneration(type);
            _generationContext.AddGeneratedType(type);
            ExecuteWithTypeContextLogging(() => { files = GenerateType(type); });
            _generationContext.EndTypeGeneration();

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

            return GenerateNotMarkedType(type, Options.BaseOutputDirectory);
        }
        
        /// <summary>
        /// Generates TypeScript files for types that are not marked with an ExportTs... attribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDirectory"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        private IEnumerable<string> GenerateNotMarkedType(Type type, string outputDirectory)
        {
            if (Options.IsTypeBlacklisted(type)) return Enumerable.Empty<string>();
            
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsClass || typeInfo.IsStruct())
            {
                return Options.ExportTypesAsInterfacesByDefault
                    ? GenerateInterface(type, new ExportTsInterfaceAttribute { OutputDir = outputDirectory })
                    : GenerateClass(type, new ExportTsClassAttribute { OutputDir = outputDirectory });
            }
            
            if (typeInfo.IsInterface)
                return GenerateInterface(type, new ExportTsInterfaceAttribute { OutputDir = outputDirectory });

            if (typeInfo.IsEnum)
                return GenerateEnum(type, new ExportTsEnumAttribute { OutputDir = outputDirectory });

            throw new CoreException($"Generated type must be a C# class, interface, struct or enum. Error when generating type {type.FullName}");
        }

        /// <summary>
        /// Generates a TypeScript class file from a class type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="classAttribute"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        private IEnumerable<string> GenerateClass(Type type, ExportTsClassAttribute classAttribute)
        {
            string outputDir = classAttribute.OutputDir;
            IEnumerable<string> dependenciesGenerationResult = GenerateTypeDependencies(type, outputDir);

            // get text for sections

            var tsCustomBaseAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsCustomBaseAttribute>(type);
            var tsIgnoreBaseAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsIgnoreBaseAttribute>(type);
            var extendsText = "";
            var implementsText = "";

            if (tsCustomBaseAttribute != null)
            {
                extendsText = string.IsNullOrEmpty(tsCustomBaseAttribute.Base) ? "" : _templateService.GetExtendsText(tsCustomBaseAttribute.Base);
                var implementedInterfaceNames = GetNotNullOrEmptyImplementedInterfaceNames(tsCustomBaseAttribute);
                implementsText = implementedInterfaceNames.None() ? "" : _templateService.GetImplementsText(implementedInterfaceNames);
            }
            else if (tsIgnoreBaseAttribute == null)
            {
                if (!type.IsStruct()) extendsText = _tsContentGenerator.GetExtendsForClassesText(type);
                implementsText = _tsContentGenerator.GetImplementsText(type);
            }

            string importsText = _tsContentGenerator.GetImportsText(type, outputDir);
            string propertiesText = GetClassPropertiesText(type);

            // generate the file content

            string tsTypeName = _typeService.GetTsTypeName(type, true);
            string tsTypeNameFirstPart = tsTypeName.RemoveTsTypeNameGenericComponent();
            string filePath = GetFilePath(type, outputDir);
            string filePathRelative = GetRelativeFilePath(type, outputDir);
            string customInFileHead = _tsContentGenerator.GetCustomHead(filePath);
            string customAttributeHead = classAttribute.CustomHeader;
            string customHead = string.Join(Environment.NewLine, new[] { customInFileHead, customAttributeHead }.Where(i => !string.IsNullOrWhiteSpace(i)));
            string customInFileBody = _tsContentGenerator.GetCustomBody(filePath, Options.TabLength);
            string customAttributeBody = classAttribute.CustomBody;
            string customBody = string.Join(Environment.NewLine, new[] { customInFileBody, customAttributeBody }.Where(i => !string.IsNullOrWhiteSpace(i)));
            var tsDoc = GetTsDocForType(type);

            var content = _typeService.UseDefaultExport(type) ?
                _templateService.FillClassDefaultExportTemplate(importsText, tsTypeName, tsTypeNameFirstPart, extendsText, implementsText, propertiesText, tsDoc, customHead, customBody, Options.FileHeading) :
                _templateService.FillClassTemplate(importsText, tsTypeName, extendsText, implementsText, propertiesText, tsDoc, customHead, customBody, Options.FileHeading);

            // write TypeScript file
            FileContentGenerated?.Invoke(this, new FileContentGeneratedArgs(type, filePath, content));
            return new[] { filePathRelative }.Concat(dependenciesGenerationResult).ToList();
        }

        /// <summary>
        /// Generates a TypeScript interface file from a class type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceAttribute"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        private IEnumerable<string> GenerateInterface(Type type, ExportTsInterfaceAttribute interfaceAttribute)
        {
            string outputDir = interfaceAttribute.OutputDir;
            IEnumerable<string> dependenciesGenerationResult = GenerateTypeDependencies(type, outputDir);

            // get text for sections

            var tsCustomBaseAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsCustomBaseAttribute>(type);
            var tsIgnoreBaseAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsIgnoreBaseAttribute>(type);
            var extendsText = "";

            if (tsCustomBaseAttribute != null)
            {
                extendsText = string.IsNullOrEmpty(tsCustomBaseAttribute.Base) ? "" : _templateService.GetExtendsText(tsCustomBaseAttribute.Base);
                EnsureInterfaceDoesNotImplementInterfaces(type, tsCustomBaseAttribute);
            }
            else if (tsIgnoreBaseAttribute == null && !type.IsStruct())
            {
                extendsText = _tsContentGenerator.GetExtendsForInterfacesText(type);
            }

            string importsText = _tsContentGenerator.GetImportsText(type, outputDir);
            string propertiesText = GetInterfacePropertiesText(type);

            // generate the file content

            string tsTypeName = _typeService.GetTsTypeName(type, true);
            string tsTypeNameFirstPart = tsTypeName.RemoveTsTypeNameGenericComponent();
            string filePath = GetFilePath(type, outputDir);
            string filePathRelative = GetRelativeFilePath(type, outputDir);
            string customInFileHead = _tsContentGenerator.GetCustomHead(filePath);
            string customAttributeHead = interfaceAttribute.CustomHeader;
            string customHead = string.Join(Environment.NewLine, new[] { customInFileHead, customAttributeHead }.Where(i => !string.IsNullOrWhiteSpace(i)));
            string customInFileBody = _tsContentGenerator.GetCustomBody(filePath, Options.TabLength);
            string customAttributeBody = interfaceAttribute.CustomBody;
            string customBody = string.Join(Environment.NewLine, new[] { customInFileBody, customAttributeBody }.Where(i => !string.IsNullOrWhiteSpace(i)));
            var tsDoc = GetTsDocForType(type);

            var content = _typeService.UseDefaultExport(type) ?
                    _templateService.FillInterfaceDefaultExportTemplate(importsText, tsTypeName, tsTypeNameFirstPart, extendsText, propertiesText, tsDoc, customHead, customBody, Options.FileHeading) :
                    _templateService.FillInterfaceTemplate(importsText, tsTypeName, extendsText, propertiesText, tsDoc, customHead, customBody, Options.FileHeading);

            // write TypeScript file
            FileContentGenerated?.Invoke(this, new FileContentGeneratedArgs(type, filePath, content));
            return new[] { filePathRelative }.Concat(dependenciesGenerationResult).ToList();
        }

        private static void EnsureInterfaceDoesNotImplementInterfaces(Type type, TsCustomBaseAttribute tsCustomBaseAttribute)
        {
            if (tsCustomBaseAttribute.ImplementedInterfaces.Any())
                throw new InvalidOperationException($"TS interface ({type.FullName}) cannot implement interfaces.");
        }

        private static List<string> GetNotNullOrEmptyImplementedInterfaceNames(TsCustomBaseAttribute tsCustomBaseAttribute)
            => tsCustomBaseAttribute.ImplementedInterfaces
                .Select(x => x.Name)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
            
        /// <summary>
        /// Generates a TypeScript enum file from a class type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="enumAttribute"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        private IEnumerable<string> GenerateEnum(Type type, ExportTsEnumAttribute enumAttribute)
        {
            string valuesText = GetEnumMembersText(type, enumAttribute.AsUnionType);

            // create TypeScript source code for the enum

            string tsEnumName = _typeService.GetTsTypeName(type, true);
            string filePath = GetFilePath(type, enumAttribute.OutputDir);
            string filePathRelative = GetRelativeFilePath(type, enumAttribute.OutputDir);
            string customHead = _tsContentGenerator.GetCustomHead(filePath);
            string customBody = _tsContentGenerator.GetCustomBody(filePath, Options.TabLength);
            var tsDoc = GetTsDocForType(type);

            string enumText = _typeService.UseDefaultExport(type) ? 
                _templateService.FillEnumDefaultExportTemplate("", tsEnumName, valuesText, tsDoc, enumAttribute.IsConst, enumAttribute.AsUnionType, Options.FileHeading) :
                _templateService.FillEnumTemplate("", tsEnumName, valuesText, enumAttribute.IsConst, enumAttribute.AsUnionType, tsDoc, customHead, customBody, Options.FileHeading);

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
        /// <param name="type"></param>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        private string GetClassPropertyText(Type type, MemberInfo memberInfo)
        {
            LogClassPropertyWarnings(memberInfo);
            if (_typeService.MemberTypeContainsBlacklistedType(memberInfo)) ThrowMemberTypeIsBlacklisted(memberInfo);
            
            string modifiers = Options.ExplicitPublicAccessor ? "public " : "";

            if (IsStaticTsProperty(memberInfo)) modifiers += "static ";
            if (IsReadonlyTsProperty(memberInfo)) modifiers += "readonly ";

            var nameAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsMemberNameAttribute>(memberInfo);
            string name = nameAttribute?.Name ?? Options.PropertyNameConverters.Convert(memberInfo.Name, memberInfo);
            string typeName = _typeService.GetTsTypeName(memberInfo);
            IEnumerable<string> typeUnions = _typeService.GetTypeUnions(memberInfo);

            var tsDoc = GetTsDocForMember(type, memberInfo);
            bool isOptional = _metadataReaderFactory.GetInstance().GetAttribute<TsOptionalAttribute>(memberInfo) != null;
            var isNullable = memberInfo.IsNullable();
            if (isNullable && Options.CsNullableTranslation == StrictNullTypeUnionFlags.Optional)
            {
                isOptional = true;
            }

            // try to get default value from TsDefaultValueAttribute
            var defaultValueAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsDefaultValueAttribute>(memberInfo);
            if (defaultValueAttribute != null)
                return _templateService.FillClassPropertyTemplate(modifiers, name, typeName, typeUnions, isOptional, tsDoc, defaultValueAttribute.DefaultValue);

            // try to get default value from the member's default value
            string valueText = _tsContentGenerator.GetMemberValueText(memberInfo);
            if (!string.IsNullOrWhiteSpace(valueText))
                return _templateService.FillClassPropertyTemplate(modifiers, name, typeName, typeUnions, isOptional, tsDoc, valueText);

            // try to get default value from Options.DefaultValuesForTypes
            if (Options.DefaultValuesForTypes.Any() && Options.DefaultValuesForTypes.ContainsKey(typeName))
                return _templateService.FillClassPropertyTemplate(modifiers, name, typeName, typeUnions, isOptional, tsDoc, Options.DefaultValuesForTypes[typeName]);

            return _templateService.FillClassPropertyTemplate(modifiers, name, typeName, typeUnions, isOptional, tsDoc);
        }

        private static void ThrowMemberTypeIsBlacklisted(MemberInfo memberInfo)
        {
            throw new CoreException($"Member '{memberInfo.DeclaringType.FullName}.{memberInfo.Name}'" +
                                                   $" contains a blacklisted type. Possible solutions:" +
                                                   $"{Environment.NewLine}1. Remove the type from blacklist." +
                                                   $"{Environment.NewLine}2. Remove the member." +
                                                   $"{Environment.NewLine}3. Add TsTypeAttribute to the member." +
                                                   $"{Environment.NewLine}4. Create custom type mapping for the blacklisted type.");
        }

        private string GetTsDocForMember(Type type, MemberInfo memberInfo)
        {
            if (_generationContext.DoesNotContainXmlDocForAssembly(type.Assembly)) return "";
            var xmlDoc = _generationContext.GetXmlDocForMember(type, memberInfo);
            var result = xmlDoc != null ? XmlDocToTsDocConverter.Convert(xmlDoc) : "";
            
            return !string.IsNullOrEmpty(result)
                ? result.AddIndentation(Options.TabLength) + Environment.NewLine
                : result;
        }
        
        private string GetTsDocForType(Type type)
        {
            if (_generationContext.DoesNotContainXmlDocForAssembly(type.Assembly)) return "";
            var xmlDoc = _generationContext.GetXmlDocForType(type);
            
            return xmlDoc != null
                ? XmlDocToTsDocConverter.Convert(xmlDoc) + Environment.NewLine
                : "";
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
                .Aggregate(propertiesText, (current, memberInfo) => current + GetClassPropertyText(type, memberInfo));

            return RemoveLastLineEnding(propertiesText);
        }

        /// <summary>
        /// Gets TypeScript interface property definition source code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        private string GetInterfacePropertyText(Type type, MemberInfo memberInfo)
        {
            LogInterfacePropertyWarnings(memberInfo);
            if (_typeService.MemberTypeContainsBlacklistedType(memberInfo)) ThrowMemberTypeIsBlacklisted(memberInfo);
            
            string modifiers = "";
            if (IsReadonlyTsProperty(memberInfo)) modifiers += "readonly ";
            
            var nameAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsMemberNameAttribute>(memberInfo);
            string name = nameAttribute?.Name ?? Options.PropertyNameConverters.Convert(memberInfo.Name, memberInfo);

            string typeName = _typeService.GetTsTypeName(memberInfo);
            IEnumerable<string> typeUnions = _typeService.GetTypeUnions(memberInfo);

            var tsDoc = GetTsDocForMember(type, memberInfo);
            bool isOptional = _metadataReaderFactory.GetInstance().GetAttribute<TsOptionalAttribute>(memberInfo) != null;
            var isNullable = memberInfo.IsNullable();
            if (isNullable && Options.CsNullableTranslation == StrictNullTypeUnionFlags.Optional)
            {
                isOptional = true;
            }

            return _templateService.FillInterfacePropertyTemplate(modifiers, name, typeName, typeUnions, isOptional, tsDoc);
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
                .Aggregate(propertiesText, (current, memberInfo) => current + GetInterfacePropertyText(type, memberInfo));

            return RemoveLastLineEnding(propertiesText);
        }

        /// <summary>
        /// Gets TypeScript enum member definition source code
        /// </summary>
        /// <param name="fieldInfo">MemberInfo for an enum value</param>
        /// <param name="asUnionType">defines if generated Text should ab applicable for union types</param>
        /// <returns></returns>
        private string GetEnumMemberText(FieldInfo fieldInfo, bool asUnionType)
        {
            Type type = fieldInfo.DeclaringType;

            var tsDoc = GetTsDocForMember(type, fieldInfo);
            string name = Options.EnumValueNameConverters.Convert(fieldInfo.Name, fieldInfo);
            var stringInitializersAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsStringInitializersAttribute>(type);
            
            if ((Options.EnumStringInitializers && (stringInitializersAttribute == null || stringInitializersAttribute.Enabled)) ||
                (stringInitializersAttribute != null && stringInitializersAttribute.Enabled))
            {
                string enumValueString = Options.EnumStringInitializersConverters.Convert(fieldInfo.Name, fieldInfo);
                return asUnionType ? _templateService.FillEnumUnionTypeValueTemplate(name) : _templateService.FillEnumValueTemplate(name, enumValueString, tsDoc);
            }

            object enumValue = fieldInfo.GetValue(null);
            object enumValueAsUnderlyingType = Convert.ChangeType(enumValue, Enum.GetUnderlyingType(type));
            return asUnionType ? _templateService.FillEnumUnionTypeValueTemplate(name) : _templateService.FillEnumValueTemplate(name, enumValueAsUnderlyingType, tsDoc);
        }

        /// <summary>
        /// Gets TypeScript enum member definition source code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="asUnionType"></param>
        /// <returns></returns>
        private string GetEnumMembersText(Type type, bool asUnionType)
        {
            var valuesText = "";
            IEnumerable<FieldInfo> fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            valuesText += fieldInfos.Aggregate(valuesText, (current, fieldInfo) => current + GetEnumMemberText(fieldInfo, asUnionType));

            return asUnionType ? TrimForEnumUnionTypeValues(valuesText) : RemoveLastLineEnding(valuesText);
        }

        /// <summary>
        /// Generates type dependencies for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        private IEnumerable<string> GenerateTypeDependencies(Type type, string outputDir)
        {
            var generatedFiles = new List<string>();
            var typeDependencies = _typeDependencyService.GetTypeDependencies(type);

            foreach (var typeDependencyInfo in typeDependencies)
            {
                var typeDependency = typeDependencyInfo.Type;
                if (typeDependency.HasExportAttribute(_metadataReaderFactory.GetInstance()) || _generationContext.IsTypeGenerated(typeDependency)) continue;
                
                var defaultOutputAttribute = typeDependencyInfo.MemberAttributes
                        ?.FirstOrDefault(a => a is TsDefaultTypeOutputAttribute)
                    as TsDefaultTypeOutputAttribute;

                var defaultOutputDir = defaultOutputAttribute?.OutputDir ?? outputDir;
                
                _generationContext.AddGeneratedType(typeDependency);
                
                try
                {
                    generatedFiles.AddRange(GenerateNotMarkedType(typeDependency, defaultOutputDir));
                }
                catch (Exception ex)
                {
                    throw new CoreException($"Error generating type dependencies for '{type.FullName}'", ex);
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
                    throw new CoreException(e.Message + "; inside type: " +
                                            string.Join(", in ", _generationContext.GetTypeGenerationStack().Select(t => t.FullName).ToList()),
                        e);
            }
        }

        private static string RemoveLastLineEnding(string propertiesText)
        {
            return propertiesText.TrimEnd('\r', '\n');
        }

        private static string TrimForEnumUnionTypeValues(string propertiesText)
        {
            return RemoveLastLineEnding(propertiesText).Trim().TrimEnd('|').Trim();
        }
    }
}
