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

namespace TypeGen.Core
{
    /// <summary>
    /// Class used for generating TypeScript files from C# files
    /// </summary>
    public class Generator : IGenerator
    {
        // dependencies

        private readonly TypeService _typeService;
        private readonly TypeDependencyService _typeDependencyService;
        private readonly TemplateService _templateService;
        private readonly TsContentGenerator _tsContentGenerator;
        private readonly FileSystem _fileSystem;
        private GeneratorOptions _options;

        /// <summary>
        /// Generator options. Cannot be null.
        /// </summary>
        public GeneratorOptions Options {
            get
            {
                return _options;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Options));
                }
                _options = value;
                if (_templateService != null) _templateService.TabLength = value.TabLength;
            }
        }

        public Generator()
        {
            Options = new GeneratorOptions();

            _fileSystem = new FileSystem();
            _typeService = new TypeService();
            _typeDependencyService = new TypeDependencyService(_typeService);
            _templateService = new TemplateService(Options.TabLength);

            _tsContentGenerator = new TsContentGenerator(_typeDependencyService,
                _typeService,
                _templateService,
                _fileSystem,
                new TsContentParser(_fileSystem));
        }

        /// <summary>
        /// Generates TypeScript files for C# files in an assembly
        /// </summary>
        /// <param name="assembly"></param>
        public void Generate(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                Generate(type);
            }
        }

        /// <summary>
        /// Generate TypeScript files for a given type
        /// </summary>
        /// <param name="type"></param>
        public void Generate(Type type)
        {
            var classAttribute = type.GetCustomAttribute<ExportTsClassAttribute>();
            if (classAttribute != null)
            {
                GenerateClass(type, classAttribute);
            }

            var interfaceAttribute = type.GetCustomAttribute<ExportTsInterfaceAttribute>();
            if (interfaceAttribute != null)
            {
                GenerateInterface(type, interfaceAttribute);
            }

            var enumAttribute = type.GetCustomAttribute<ExportTsEnumAttribute>();
            if (enumAttribute != null)
            {
                GenerateEnum(type, enumAttribute);
            }
        }

        /// <summary>
        /// Generates a TypeScript class file from a class type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="classAttribute"></param>
        private void GenerateClass(Type type, ExportTsClassAttribute classAttribute)
        {
            // get text for sections

            string extendsText = _tsContentGenerator.GetExtendsText(type, Options.TypeNameConverters);
            string importsText = ResolveTypeImports(type, classAttribute.OutputDir);
            string propertiesText = GetClassPropertiesText(type);

            // generate the file content

            string tsClassName = _typeService.GetTsTypeName(type, Options.TypeNameConverters);
            string filePath = GetFilePath(type, classAttribute.OutputDir);
            string customHead = _tsContentGenerator.GetCustomHead(filePath);
            string customBody = _tsContentGenerator.GetCustomBody(filePath, Options.TabLength);

            string classText = _templateService.FillClassTemplate(importsText, tsClassName, extendsText, propertiesText, customHead, customBody);

            // write TypeScript file

            _fileSystem.SaveFile(filePath, classText);
        }

        /// <summary>
        /// Generates a TypeScript interface file from a class type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceAttribute"></param>
        private void GenerateInterface(Type type, ExportTsInterfaceAttribute interfaceAttribute)
        {
            // get text for sections

            string extendsText = _tsContentGenerator.GetExtendsText(type, Options.TypeNameConverters);
            string importsText = ResolveTypeImports(type, interfaceAttribute.OutputDir);
            string propertiesText = GetInterfacePropertiesText(type);

            // generate the file content

            string tsInterfaceName = _typeService.GetTsTypeName(type, Options.TypeNameConverters);
            string filePath = GetFilePath(type, interfaceAttribute.OutputDir);
            string customHead = _tsContentGenerator.GetCustomHead(filePath);
            string customBody = _tsContentGenerator.GetCustomBody(filePath, Options.TabLength);

            string interfaceText = _templateService.FillInterfaceTemplate(importsText, tsInterfaceName, extendsText, propertiesText, customHead, customBody);

            // write TypeScript file

            _fileSystem.SaveFile(filePath, interfaceText);
        }

        /// <summary>
        /// Generates a TypeScript enum file from a class type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="enumAttribute"></param>
        private void GenerateEnum(Type type, ExportTsEnumAttribute enumAttribute)
        {
            string valuesText = GetEnumValuesText(type);

            // create TypeScript source code for the enum

            string tsEnumName = _typeService.GetTsTypeName(type, Options.TypeNameConverters);
            string filePath = GetFilePath(type, enumAttribute.OutputDir);

            string enumText = _templateService.FillEnumTemplate("", tsEnumName, valuesText);

            // write TypeScript file

            _fileSystem.SaveFile(filePath, enumText);
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

            var defaultValueAttribute = memberInfo.GetCustomAttribute<TsDefaultValueAttribute>();
            if (defaultValueAttribute != null)
            {
                return _templateService.FillClassPropertyWithDefaultValueTemplate(accessorText, name, defaultValueAttribute.DefaultValue);
            }

            string typeName = GetTsTypeNameForMember(memberInfo);
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

            if (propertiesText != "")
            {
                // remove the last new line symbol
                propertiesText = propertiesText.Remove(propertiesText.Length - 2);
            }

            return propertiesText;
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

            string typeName = GetTsTypeNameForMember(memberInfo);

            return _templateService.FillInterfacePropertyTemplate(name, typeName);
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

            if (propertiesText != "")
            {
                // remove the last new line symbol
                propertiesText = propertiesText.Remove(propertiesText.Length - 2);
            }

            return propertiesText;
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

            if (valuesText != "")
            {
                // remove the last new line symbol
                valuesText = valuesText.Remove(valuesText.Length - 2);
            }

            return valuesText;
        }

        /// <summary>
        /// Returns TypeScript imports declaration source code.
        /// Generates TS files for dependencies if needed.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir">The passed type's output directory</param>
        /// <returns></returns>
        private string ResolveTypeImports(Type type, string outputDir)
        {
            GenerateTypeDependencies(type, outputDir);
            return _tsContentGenerator.GetImportsText(type, outputDir, Options.FileNameConverters, Options.TypeNameConverters);
        }

        /// <summary>
        /// Generates type dependencies' files for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        private void GenerateTypeDependencies(Type type, string outputDir)
        {
            IEnumerable<TypeDependencyInfo> typeDependencies = _typeDependencyService.GetTypeDependencies(type);

            foreach (TypeDependencyInfo typeDependencyInfo in typeDependencies)
            {
                Type typeDependency = typeDependencyInfo.Type;

                var dependencyClassAttribute = typeDependency.GetCustomAttribute<ExportTsClassAttribute>();
                var dependencyInterfaceAttribute = typeDependency.GetCustomAttribute<ExportTsInterfaceAttribute>();
                var dependencyEnumAttribute = typeDependency.GetCustomAttribute<ExportTsEnumAttribute>();

                // dependency type TypeScript file generation

                // dependency type NOT in the same assembly, but HAS ExportTsX attribute
                if (typeDependency.AssemblyQualifiedName != type.AssemblyQualifiedName
                    && (dependencyClassAttribute != null || dependencyEnumAttribute != null || dependencyInterfaceAttribute != null))
                {
                    Generate(typeDependency);
                }

                // dependency DOESN'T HAVE an ExportTsX attribute
                if (dependencyClassAttribute == null && dependencyEnumAttribute == null && dependencyInterfaceAttribute == null)
                {
                    var defaultOutputAttribute = typeDependencyInfo.MemberAttributes
                        ?.FirstOrDefault(a => a is TsDefaultTypeOutputAttribute)
                        as TsDefaultTypeOutputAttribute;

                    string defaultOutputDir = defaultOutputAttribute?.OutputDir ?? outputDir;

                    if (typeDependency.IsClass)
                    {
                        GenerateClass(typeDependency, new ExportTsClassAttribute { OutputDir = defaultOutputDir });
                    }
                    else if (typeDependency.IsEnum)
                    {
                        GenerateEnum(typeDependency, new ExportTsEnumAttribute { OutputDir = defaultOutputDir });
                    }
                    else
                    {
                        throw new CoreException($"Could not generate TypeScript file for C# type '{typeDependency.FullName}'. Specified type is not a class or enum type. Dependent type: '{type.FullName}'.");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the TypeScript type name to generate for a member
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        private string GetTsTypeNameForMember(MemberInfo memberInfo)
        {
            var typeAttribute = memberInfo.GetCustomAttribute<TsTypeAttribute>();
            if (typeAttribute != null)
            {
                if (typeAttribute.TypeName.IsNullOrWhitespace())
                {
                    throw new CoreException($"No type specified in TsType attribute for member '{memberInfo.Name}' declared in '{memberInfo.DeclaringType?.FullName}'");
                }
                return typeAttribute.TypeName;
            }
            
            return _typeService.GetTsTypeName(memberInfo, Options.TypeNameConverters);
        }

        /// <summary>
        /// Gets the output TypeScript file path based on a type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        private string GetFilePath(Type type, string outputDir)
        {
            string typeName = type.Name.RemoveTypeArity();
            string fileName = Options.FileNameConverters.Convert(typeName, type);

            if (!string.IsNullOrEmpty(Options.TypeScriptFileExtension))
            {
                fileName += $".{Options.TypeScriptFileExtension}";
            }

            fileName = string.IsNullOrEmpty(outputDir)
                ? fileName
                : $"{outputDir.NormalizePath()}\\{fileName}";

            string separator = string.IsNullOrEmpty(Options.BaseOutputDirectory) ? "" : "\\";
            return Options.BaseOutputDirectory + separator + fileName;
        }
    }
}
