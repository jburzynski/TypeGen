using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TypeGen.Core.Business;
using TypeGen.Core.Extensions;
using TypeGen.Core.Storage;
using TypeGen.Core.TypeAnnotations;
using TypeGen.Core.Utils;
using TypeGen.Core.Validation;

namespace TypeGen.Core
{
    /// <summary>
    /// Class used for generating TypeScript files from C# types
    /// </summary>
    public class Generator : IGenerator
    {
        // dependencies

        private readonly ITypeService _typeService;
        private readonly ITypeDependencyService _typeDependencyService;
        private readonly ITemplateService _templateService;
        private readonly ITsContentGenerator _tsContentGenerator;
        private readonly IFileSystem _fileSystem;
        private GeneratorOptions _options;

        // per-generation shared variables

        // type collections, to keep track of what types have been generated in the current session
        private readonly GenerationContext _generationContext;

        /// <summary>
        /// Generator options. Cannot be null.
        /// </summary>
        public GeneratorOptions Options
        {
            get => _options;

            set
            {
                _options = value ?? throw new ArgumentNullException(nameof(Options));

                if (_templateService != null)
                {
                    _templateService.GeneratorOptions = value;
                }
            }
        }

        public Generator()
        {
            Options = new GeneratorOptions();
            _generationContext = new GenerationContext();

            var internalStorage = new InternalStorage();
            _fileSystem = new FileSystem();
            _typeService = new TypeService();
            _typeDependencyService = new TypeDependencyService(_typeService);
            _templateService = new TemplateService(internalStorage) { GeneratorOptions = Options };

            _tsContentGenerator = new TsContentGenerator(_typeDependencyService,
                _typeService,
                _templateService,
                new TsContentParser(_fileSystem));
        }

        /// <summary>
        /// For unit testing (mocking FileSystem)
        /// </summary>
        /// <param name="fileSystem"></param>
        internal Generator(IFileSystem fileSystem) : this() => _fileSystem = fileSystem;

        /// <inheritdoc />
        public GenerationResult Generate(Assembly assembly)
        {
            Requires.NotNull(assembly, nameof(assembly));
            return Generate(new[] { assembly });
        }

        /// <inheritdoc />
        public GenerationResult Generate(IEnumerable<Assembly> assemblies)
        {
            Requires.NotNull(assemblies, nameof(assemblies));
            
            IEnumerable<string> files = Enumerable.Empty<string>();

            foreach (Assembly assembly in assemblies)
            {
                _generationContext.InitializeAssemblyGeneratedTypes();

                ExecuteWithTypeContextLogging(() =>
                {
                    foreach (Type type in assembly.GetLoadableTypes().GetExportMarkedTypes())
                    {
                        if (_generationContext.HasBeenGeneratedForAssembly(type)) continue;
                        files = files.Concat(Generate(type).GeneratedFiles);
                    }
                });

                _generationContext.ClearAssemblyGeneratedTypes();
            }

            if (Options.CreateIndexFile)
            {
                files = files.Concat(new[] { GenerateIndexFile(files) });
            }

            return new GenerationResult
            {
                BaseOutputDirectory = Options.BaseOutputDirectory,
                GeneratedFiles = files.Distinct()
            };
        }

        /// <inheritdoc />
        public GenerationResult Generate(Type type)
        {
            Requires.NotNull(type, nameof(type));
            
            _generationContext.InitializeTypeGeneratedTypes();
            _generationContext.Add(type);

            IEnumerable<string> files = Enumerable.Empty<string>();

            if (_generationContext.IsAssemblyContext())
            {
                files = GenerateType(type).GeneratedFiles;
            }
            else
            {
                ExecuteWithTypeContextLogging(() => { files = GenerateType(type).GeneratedFiles; });
            }

            _generationContext.ClearTypeGeneratedTypes();

            return new GenerationResult
            {
                BaseOutputDirectory = Options.BaseOutputDirectory,
                GeneratedFiles = files.Distinct()
            };
        }

        /// <summary>
        /// Generates an `index.ts` file which exports all types within the generated files
        /// </summary>
        /// <param name="generatedFiles"></param>
        private string GenerateIndexFile(IEnumerable<string> generatedFiles)
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
            _fileSystem.SaveFile(Path.Combine(Options.BaseOutputDirectory, filename), content);
            return filename;
        }

        /// <summary>
        /// Contains the actual logic of generating TypeScript files for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private GenerationResult GenerateType(Type type)
        {
            var classAttribute = type.GetTypeInfo().GetCustomAttribute<ExportTsClassAttribute>();
            var interfaceAttribute = type.GetTypeInfo().GetCustomAttribute<ExportTsInterfaceAttribute>();
            var enumAttribute = type.GetTypeInfo().GetCustomAttribute<ExportTsEnumAttribute>();

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
        /// Generates TypeScript files for types that are not marked with an ExportTs... attribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDirectory"></param>
        /// <returns></returns>
        private GenerationResult GenerateNotMarked(Type type, string outputDirectory)
        {
            if (type.GetTypeInfo().IsClass)
            {
                return GenerateClass(type, new ExportTsClassAttribute { OutputDir = outputDirectory });
            }

            if (type.GetTypeInfo().IsEnum)
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
        private GenerationResult GenerateClass(Type type, ExportTsClassAttribute classAttribute)
        {
            return GenerateClassOrInterface(type, classAttribute, null);
        }

        /// <summary>
        /// Generates a TypeScript interface file from a class type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceAttribute"></param>
        private GenerationResult GenerateInterface(Type type, ExportTsInterfaceAttribute interfaceAttribute)
        {
            return GenerateClassOrInterface(type, null, interfaceAttribute);
        }

        private GenerationResult GenerateClassOrInterface(Type type, ExportTsClassAttribute classAttribute, ExportTsInterfaceAttribute interfaceAttribute)
        {
            string outputDir = classAttribute != null ? classAttribute.OutputDir : interfaceAttribute.OutputDir;
            GenerationResult dependenciesGenerationResult = GenerateTypeDependencies(type, outputDir);

            // get text for sections

            var tsCustomBaseAttribute = type.GetTypeInfo().GetCustomAttribute<TsCustomBaseAttribute>();
            var extendsText = "";

            if (tsCustomBaseAttribute != null)
            {
                extendsText = string.IsNullOrEmpty(tsCustomBaseAttribute.Base) ? "" : _templateService.GetExtendsText(tsCustomBaseAttribute.Base);
            }
            else if (type.GetTypeInfo().GetCustomAttribute<TsIgnoreBaseAttribute>() == null)
            {
                extendsText = _tsContentGenerator.GetExtendsText(type, Options.TypeNameConverters);
            }

            string importsText = _tsContentGenerator.GetImportsText(type, outputDir, Options.FileNameConverters, Options.TypeNameConverters);
            string propertiesText = classAttribute != null ? GetClassPropertiesText(type) : GetInterfacePropertiesText(type);

            // generate the file content

            string tsTypeName = _typeService.GetTsTypeName(type, Options.TypeNameConverters, true);
            string filePath = GetFilePath(type, outputDir);
            string filePathRelative = GetRelativeFilePath(type, outputDir);
            string customHead = _tsContentGenerator.GetCustomHead(filePath);
            string customBody = _tsContentGenerator.GetCustomBody(filePath, Options.TabLength);

            string content = classAttribute != null ?
                _templateService.FillClassTemplate(importsText, tsTypeName, extendsText, propertiesText, customHead, customBody) :
                _templateService.FillInterfaceTemplate(importsText, tsTypeName, extendsText, propertiesText, customHead, customBody);

            // write TypeScript file

            _fileSystem.SaveFile(filePath, content);

            return new GenerationResult
            {
                GeneratedFiles = new[] { filePathRelative }
                    .Concat(dependenciesGenerationResult.GeneratedFiles)
            };
        }

        /// <summary>
        /// Generates a TypeScript enum file from a class type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="enumAttribute"></param>
        private GenerationResult GenerateEnum(Type type, ExportTsEnumAttribute enumAttribute)
        {
            string valuesText = GetEnumValuesText(type);

            // create TypeScript source code for the enum

            string tsEnumName = _typeService.GetTsTypeName(type, Options.TypeNameConverters, true);
            string filePath = GetFilePath(type, enumAttribute.OutputDir);
            string filePathRelative = GetRelativeFilePath(type, enumAttribute.OutputDir);

            string enumText = _templateService.FillEnumTemplate("", tsEnumName, valuesText, enumAttribute.IsConst);

            // write TypeScript file

            _fileSystem.SaveFile(filePath, enumText);
            return new GenerationResult { GeneratedFiles = new[] { filePathRelative } };
        }

        /// <summary>
        /// Gets TypeScript class property definition source code
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        private string GetClassPropertyText(MemberInfo memberInfo)
        {
            string accessorText = Options.ExplicitPublicAccessor ? "public " : "";

            var nameAttribute = memberInfo.GetCustomAttribute<TsMemberNameAttribute>();
            string name = nameAttribute?.Name ?? Options.PropertyNameConverters.Convert(memberInfo.Name);
            string typeName = _typeService.GetTsTypeName(memberInfo, Options.TypeNameConverters, Options.StrictNullChecks, Options.CsNullableTranslation);

            var defaultValueAttribute = memberInfo.GetCustomAttribute<TsDefaultValueAttribute>();
            if (defaultValueAttribute != null)
            {
                return _templateService.FillClassPropertyWithDefaultValueTemplate(accessorText, name, typeName, defaultValueAttribute.DefaultValue);
            }

            if (Options.GenerateEmptyValues.Any())
            {
                Type memberType = _typeService.GetMemberType(memberInfo);
                if (Options.GenerateEmptyValues.Contains(memberType) && EmptyValue.ExistsFor(memberType))
                {
                    string defaultValue = EmptyValue.For(memberType, Options.SingleQuotes);
                    return _templateService.FillClassPropertyWithDefaultValueTemplate(accessorText, name, typeName, defaultValue);
                }
            }

            return _templateService.FillClassPropertyTemplate(accessorText, name, typeName);
        }

        /// <summary>
        /// Gets TypeScript class properties definition source code
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetClassPropertiesText(Type type)
        {
            var propertiesText = "";
            IEnumerable<MemberInfo> memberInfos = _typeService.GetTsExportableMembers(type);

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
            var nameAttribute = memberInfo.GetCustomAttribute<TsMemberNameAttribute>();
            string name = nameAttribute?.Name ?? Options.PropertyNameConverters.Convert(memberInfo.Name);

            string typeName = _typeService.GetTsTypeName(memberInfo, Options.TypeNameConverters, Options.StrictNullChecks, Options.CsNullableTranslation);
            bool isOptional = memberInfo.GetCustomAttribute<TsOptionalAttribute>() != null;

            return _templateService.FillInterfacePropertyTemplate(name, typeName, isOptional);
        }

        /// <summary>
        /// Gets TypeScript interface properties definition source code
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetInterfacePropertiesText(Type type)
        {
            var propertiesText = "";
            IEnumerable<MemberInfo> memberInfos = _typeService.GetTsExportableMembers(type);

            // create TypeScript source code for properties' definition

            propertiesText += memberInfos
                .Aggregate(propertiesText, (current, memberInfo) => current + GetInterfacePropertyText(memberInfo));

            return RemoveLastLineEnding(propertiesText);
        }

        /// <summary>
        /// Gets TypeScript enum value definition source code
        /// </summary>
        /// <param name="enumValue">an enum value (result of Enum.GetValues())</param>
        /// <returns></returns>
        private string GetEnumValueText(object enumValue)
        {
            string name = Options.EnumValueNameConverters.Convert(enumValue.ToString());
            var enumValueInt = (int)enumValue;
            return _templateService.FillEnumValueTemplate(name, enumValueInt);
        }

        /// <summary>
        /// Gets TypeScript enum values definition source code
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetEnumValuesText(Type type)
        {
            var valuesText = "";
            Array enumValues = Enum.GetValues(type);

            valuesText += enumValues.Cast<object>()
                .Aggregate(valuesText, (current, enumValue) => current + GetEnumValueText(enumValue));

            return RemoveLastLineEnding(valuesText);
        }

        /// <summary>
        /// Generates type dependencies' files for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        private GenerationResult GenerateTypeDependencies(Type type, string outputDir)
        {
            IEnumerable<string> generatedFiles = Enumerable.Empty<string>();
            IEnumerable<TypeDependencyInfo> typeDependencies = _typeDependencyService.GetTypeDependencies(type);

            foreach (TypeDependencyInfo typeDependencyInfo in typeDependencies)
            {
                Type typeDependency = typeDependencyInfo.Type;

                // dependency type TypeScript file generation

                // dependency type NOT in the same assembly, but HAS ExportTsX attribute (AND hasn't been generated yet)
                if (typeDependency.GetTypeInfo().Assembly.FullName != type.GetTypeInfo().Assembly.FullName
                    && typeDependency.HasExportAttribute()
                    && !_generationContext.HasBeenGeneratedForAssembly(typeDependency))
                {
                    _generationContext.Add(typeDependency);
                    generatedFiles = generatedFiles.Concat(Generate(typeDependency).GeneratedFiles);
                }

                // dependency HAS an ExportTsX attribute (AND hasn't been generated yet)
                if (typeDependency.HasExportAttribute() && !_generationContext.HasBeenGeneratedForAssembly(typeDependency))
                {
                    _generationContext.Add(typeDependency);
                    generatedFiles = generatedFiles.Concat(GenerateType(typeDependency).GeneratedFiles);
                }

                // dependency DOESN'T HAVE an ExportTsX attribute (AND hasn't been generated for the currently generated type yet)
                if (!typeDependency.HasExportAttribute() && !_generationContext.HasBeenGeneratedForType(typeDependency))
                {
                    var defaultOutputAttribute = typeDependencyInfo.MemberAttributes
                        ?.FirstOrDefault(a => a is TsDefaultTypeOutputAttribute)
                        as TsDefaultTypeOutputAttribute;

                    string defaultOutputDir = defaultOutputAttribute?.OutputDir ?? outputDir;

                    _generationContext.Add(typeDependency);
                    generatedFiles = generatedFiles.Concat(GenerateNotMarked(typeDependency, defaultOutputDir).GeneratedFiles);
                }
            }

            return new GenerationResult { GeneratedFiles = generatedFiles };
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
                : Path.Combine(outputDir, fileName);
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
            return Path.Combine(Options.BaseOutputDirectory ?? "", fileName);
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
