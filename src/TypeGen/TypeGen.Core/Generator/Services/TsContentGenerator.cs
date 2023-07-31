using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using TypeGen.Core.Extensions;
using TypeGen.Core.Logging;
using TypeGen.Core.Metadata;
using TypeGen.Core.TypeAnnotations;
using TypeGen.Core.Utils;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Generator.Services
{
    /// <summary>
    /// Generates TypeScript file contents
    /// </summary>
    internal class TsContentGenerator : ITsContentGenerator
    {
        private readonly ITypeDependencyService _typeDependencyService;
        private readonly ITypeService _typeService;
        private readonly ITemplateService _templateService;
        private readonly ITsContentParser _tsContentParser;
        private readonly IMetadataReaderFactory _metadataReaderFactory;
        private readonly IGeneratorOptionsProvider _generatorOptionsProvider;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        private const string KeepTsTagName = "keep-ts";
        private const string CustomHeadTagName = "custom-head";
        private const string CustomBodyTagName = "custom-body";

        private GeneratorOptions GeneratorOptions => _generatorOptionsProvider.GeneratorOptions;

        public TsContentGenerator(ITypeDependencyService typeDependencyService,
            ITypeService typeService,
            ITemplateService templateService,
            ITsContentParser tsContentParser,
            IMetadataReaderFactory metadataReaderFactory,
            IGeneratorOptionsProvider generatorOptionsProvider,
            ILogger logger)
        {
            _typeDependencyService = typeDependencyService;
            _typeService = typeService;
            _templateService = templateService;
            _tsContentParser = tsContentParser;
            _metadataReaderFactory = metadataReaderFactory;
            _generatorOptionsProvider = generatorOptionsProvider;
            _logger = logger;

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new TsJsonContractResolver(_metadataReaderFactory, GeneratorOptions)
            };
        }

        /// <summary>
        /// Gets code for the 'imports' section for a given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="outputDir">ExportTs... attribute's output dir.</param>
        /// <returns>The 'imports' section for the <see cref="type"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="type"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when <see cref="GeneratorOptions.FileNameConverters"/> or <see cref="GeneratorOptions.TypeNameConverters"/> is null.</exception>
        public string GetImportsText(Type type, string outputDir)
        {
            Requires.NotNull(type, nameof(type));
            
            if (GeneratorOptions.FileNameConverters == null)
                throw new InvalidOperationException($"{nameof(GeneratorOptions.FileNameConverters)} should not be null.");
            
            if (GeneratorOptions.TypeNameConverters == null)
                throw new InvalidOperationException($"{nameof(GeneratorOptions.TypeNameConverters)} should not be null.");

            string result = GetTypeDependencyImportsText(type, outputDir);
            result += GetCustomImportsText(type);

            if (!string.IsNullOrEmpty(result))
            {
                result += "\r\n";
            }

            return result;
        }

        /// <summary>
        /// Gets the text for the "extends" section
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetExtendsText(Type type)
        {
            Requires.NotNull(type, nameof(type));
            Requires.NotNull(GeneratorOptions.TypeNameConverters, nameof(GeneratorOptions.TypeNameConverters));
            
            Type baseType = _typeService.GetBaseType(type);
            if (baseType == null) return "";

            string baseTypeName = _typeService.GetTsTypeName(baseType, true);
            return _templateService.GetExtendsText(baseTypeName);
        }

        /// <summary>
        /// Gets the text for the "extends" section for interfaces.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetExtendsForInterfacesText(Type type)
        {
            Requires.NotNull(type, nameof(type));
            Requires.NotNull(GeneratorOptions.TypeNameConverters, nameof(GeneratorOptions.TypeNameConverters));

            IEnumerable<Type> baseTypes = _typeService.GetInterfaces(type);
            if (!baseTypes.Any()) return "";

            IEnumerable<string> baseTypeNames = baseTypes.Select(baseType => _typeService.GetTsTypeName(baseType, true));
            return _templateService.GetExtendsText(baseTypeNames);
        }

        /// <summary>
        /// Gets the text for the "implements" section
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetImplementsText(Type type)
        {
            Requires.NotNull(type, nameof(type));
            Requires.NotNull(GeneratorOptions.TypeNameConverters, nameof(GeneratorOptions.TypeNameConverters));

            IEnumerable<Type> baseTypes = _typeService.GetInterfaces(type);
            if (!baseTypes.Any()) return "";

            IEnumerable<string> baseTypeNames = baseTypes.Select(baseType => _typeService.GetTsTypeName(baseType, true));
            return _templateService.GetImplementsText(baseTypeNames);
        }
        
        /// <summary>
        /// Returns TypeScript imports source code related to type dependencies
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        private string GetTypeDependencyImportsText(Type type, string outputDir)
        {
            if (!string.IsNullOrEmpty(outputDir) && !outputDir.EndsWith("/") && !outputDir.EndsWith("\\")) outputDir += "\\";
            var result = "";
            IEnumerable<TypeDependencyInfo> typeDependencies = _typeDependencyService.GetTypeDependencies(type);

            // exclude base type dependency if TsCustomBaseAttribute is specified (it will be added in custom imports)
            if (_metadataReaderFactory.GetInstance().GetAttribute<TsCustomBaseAttribute>(type) != null)
            {
                typeDependencies = typeDependencies.Where(td => !td.IsBase);
            }

            foreach (TypeDependencyInfo typeDependencyInfo in typeDependencies)
            {
                Type typeDependency = typeDependencyInfo.Type;

                string dependencyOutputDir = GetTypeDependencyOutputDir(typeDependencyInfo, outputDir);

                // get path diff
                string pathDiff = FileSystemUtils.GetPathDiff(outputDir, dependencyOutputDir);
                pathDiff = pathDiff.StartsWith("..\\") || pathDiff.StartsWith("../") ? pathDiff : $"./{pathDiff}";

                // get type & file name
                string typeDependencyName = typeDependency.Name.RemoveTypeArity();
                string fileName = GeneratorOptions.FileNameConverters.Convert(typeDependencyName, typeDependency);

                // get file path
                string dependencyPath = Path.Combine(pathDiff.EnsurePostfix("/"), fileName);
                dependencyPath = dependencyPath.Replace('\\', '/');

                string typeName = GeneratorOptions.TypeNameConverters.Convert(typeDependencyName, typeDependency);
                
                result += _typeService.UseDefaultExport(typeDependency) ?
                    _templateService.FillImportDefaultExportTemplate(typeName, dependencyPath) :
                    _templateService.FillImportTemplate(typeName, "", dependencyPath);
            }

            return result;
        }

        /// <summary>
        /// Gets code for imports that are specified in TsTypeAttribute.ImportPath or TsCustomBaseAttribute.ImportPath properties
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetCustomImportsText(Type type)
        {
            var resultLines = new List<string>();

            resultLines.AddRange(GetCustomImportsFromCustomBase(type));
            resultLines.AddRange(GetCustomImportsFromMembers(type));

            return string.Join("", resultLines.Distinct());
        }

        private IEnumerable<string> GetCustomImportsFromMembers(Type type)
        {
            IEnumerable<MemberInfo> members = type.GetTsExportableMembers(_metadataReaderFactory.GetInstance());

            IEnumerable<TsTypeAttribute> typeAttributes = members
                .Select(memberInfo => _metadataReaderFactory.GetInstance().GetAttribute<TsTypeAttribute>(memberInfo))
                .Where(tsTypeAttribute => !string.IsNullOrEmpty(tsTypeAttribute?.ImportPath))
                .Distinct(new TsTypeAttributeComparer());

            foreach (TsTypeAttribute attribute in typeAttributes)
            {
                yield return FillCustomImportTemplate(attribute.FlatTypeName, attribute.ImportPath, attribute.OriginalTypeName, attribute.IsDefaultExport);
            }
        }

        private IEnumerable<string> GetCustomImportsFromCustomBase(Type type)
        {
            var tsCustomBaseAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsCustomBaseAttribute>(type);
            if (tsCustomBaseAttribute == null) yield break;

            if (ImportPathIsNotNullOrEmpty(tsCustomBaseAttribute))
                yield return FillCustomImportTemplate(tsCustomBaseAttribute.Base, tsCustomBaseAttribute.ImportPath, tsCustomBaseAttribute.OriginalTypeName, tsCustomBaseAttribute.IsDefaultExport);

            foreach (var implementedInterface in tsCustomBaseAttribute.ImplementedInterfaces
                         .Where(x => !string.IsNullOrEmpty(x.ImportPath)))
            {
                yield return FillCustomImportTemplate(implementedInterface.Name, implementedInterface.ImportPath, implementedInterface.OriginalTypeName, implementedInterface.IsDefaultExport);
            }
        }

        private static bool ImportPathIsNullOrEmpty(TsCustomBaseAttribute tsCustomBaseAttribute)
            => string.IsNullOrEmpty(tsCustomBaseAttribute.ImportPath);

        private static bool ImportPathIsNotNullOrEmpty(TsCustomBaseAttribute tsCustomBaseAttribute)
            => !ImportPathIsNullOrEmpty(tsCustomBaseAttribute);

        private string FillCustomImportTemplate(string typeName, string importPath, string originalTypeName, bool isDefaultExport)
        {
            bool withOriginalTypeName = !string.IsNullOrEmpty(originalTypeName);

            string name = withOriginalTypeName ? originalTypeName : typeName;
            string typeAlias = withOriginalTypeName ? typeName : null;
            
            return isDefaultExport ? _templateService.FillImportDefaultExportTemplate(name, importPath) : 
                _templateService.FillImportTemplate(name, typeAlias, importPath);
        }

        /// <summary>
        /// Gets the output directory for a type dependency
        /// </summary>
        /// <param name="typeDependencyInfo"></param>
        /// <param name="parentTypeOutputDir"></param>
        /// <returns></returns>
        private string GetTypeDependencyOutputDir(TypeDependencyInfo typeDependencyInfo, string parentTypeOutputDir)
        {
            var classAttribute = _metadataReaderFactory.GetInstance().GetAttribute<ExportTsClassAttribute>(typeDependencyInfo.Type);
            var interfaceAttribute = _metadataReaderFactory.GetInstance().GetAttribute<ExportTsInterfaceAttribute>(typeDependencyInfo.Type);
            var enumAttribute = _metadataReaderFactory.GetInstance().GetAttribute<ExportTsEnumAttribute>(typeDependencyInfo.Type);

            if (classAttribute == null && enumAttribute == null && interfaceAttribute == null)
            {
                TsDefaultTypeOutputAttribute defaultTypeOutputAttribute = typeDependencyInfo.MemberAttributes
                    ?.SingleOrDefault(a => a.GetType() == typeof(TsDefaultTypeOutputAttribute))
                    as TsDefaultTypeOutputAttribute;

                return defaultTypeOutputAttribute?.OutputDir ?? parentTypeOutputDir;
            }

            return classAttribute?.OutputDir
                    ?? interfaceAttribute?.OutputDir
                    ?? enumAttribute?.OutputDir;
        }

        /// <summary>
        /// Gets custom code for a TypeScript file given by filePath.
        /// Returns an empty string if a file does not exist.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="indentSize"></param>
        /// <returns></returns>
        public string GetCustomBody(string filePath, int indentSize)
        {
            Requires.NotNull(filePath, nameof(filePath));
            
            string content = _tsContentParser.GetTagContent(filePath, indentSize, KeepTsTagName, CustomBodyTagName);
            string tab = StringUtils.GetTabText(indentSize);

            return string.IsNullOrEmpty(content)
                ? ""
                : $"\r\n\r\n{tab}//<{CustomBodyTagName}>\r\n{tab}{content}{tab}//</{CustomBodyTagName}>";
        }

        /// <summary>
        /// Gets custom code for a TypeScript file given by filePath.
        /// Returns an empty string if a file does not exist.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetCustomHead(string filePath)
        {
            Requires.NotNull(filePath, nameof(filePath));
            
            string content = _tsContentParser.GetTagContent(filePath, 0, CustomHeadTagName);
            return string.IsNullOrEmpty(content)
                ? ""
                : $"//<{CustomHeadTagName}>\r\n{content}//</{CustomHeadTagName}>\r\n\r\n";
        }

        /// <summary>
        /// Gets text to be used as a member value
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns>The text to be used as a member value. Null if the member has no value or value cannot be determined.</returns>
        public string GetMemberValueText(MemberInfo memberInfo)
        {
            var temp = memberInfo.Name;
            if (memberInfo.DeclaringType == null) return null;

            try
            {
                object instance = memberInfo.IsStatic() ? null : ActivatorUtils.CreateInstanceAutoFillGenericParameters(memberInfo.DeclaringType);
                if (instance != null)
                {
                    memberInfo = instance.GetType().GetMember(memberInfo.Name).First(); // Properties and fields can't overload, right?
                }
                var valueObj = new object();
                object valueObjGuard = valueObj;
                bool isConstant = false;

                GetMemberValue(memberInfo, instance, out valueObj, out isConstant);

                // if only default values for constants are allowed
                if (GeneratorOptions.CsDefaultValuesForConstantsOnly && !isConstant) return null;

                // if valueObj hasn't been assigned in the switch
                if (valueObj == valueObjGuard) return null;
                
                // if valueObj's value is the default value for its type
                if (valueObj == null || valueObj.Equals(TypeUtils.GetDefaultValue(valueObj.GetType()))) return null;

                var valueType = valueObj.GetType();
                string memberType = _typeService.GetTsTypeName(memberInfo).GetTsTypeUnion(0);
                string quote = GeneratorOptions.SingleQuotes ? "'" : "\"";


                switch (valueObj)
                {
                    case Guid valueGuid when memberType == "string":
                        return quote + valueGuid + quote;
                    case DateTime valueDateTime when memberType == "Date":
                        return $@"new Date({quote}{valueDateTime.ToString("o", CultureInfo.InvariantCulture)}{quote})";
                    case DateTime valueDateTime when memberType == "string":
                        return quote + valueDateTime.ToString("o", CultureInfo.InvariantCulture) + quote;
                    case DateTimeOffset valueDateTimeOffset when memberType == "Date":
                        return $@"new Date({quote}{valueDateTimeOffset.ToString("o", CultureInfo.InvariantCulture)}{quote})";
                    case DateTimeOffset valueDateTimeOffset when memberType == "string":
                        return quote + valueDateTimeOffset.ToString("o", CultureInfo.InvariantCulture) + quote;
                    default:
                        var serializedValue = JsonConvert.SerializeObject(valueObj, _jsonSerializerSettings).Replace("\"", quote);
                        if (!_typeService.IsCollectionType(valueType) &&
                            !_typeService.IsDictionaryType(valueType) &&
                            _typeService.IsTsClass(valueType) && // Make sure it's not a list, array, or other special type
                            !valueType.GetTypeInfo().IsValueType && // Ignore value types
                            valueType.GetConstructor(Type.EmptyTypes) != null) // Make sure the type has a default constructor to use for this
                        {
                            _logger?.Log($"Checking type {valueType.FullName} for constructor usage", LogLevel.Info);
                            var defaultCtorValueType = Activator.CreateInstance(valueType);
                            if (defaultCtorValueType != null && memberwiseEquals(valueObj, defaultCtorValueType))
                            {
                                return $@"new {_typeService.GetTsTypeName(memberInfo)}()";
                            }
                        }
                        return serializedValue;
                }
            }
            catch (MissingMethodException e)
            {
                _logger?.Log($"Cannot determine the default value for member '{memberInfo.DeclaringType.FullName}.{memberInfo.Name}', because type '{memberInfo.DeclaringType.FullName}' has no default constructor.", LogLevel.Debug);
            }
            catch (ArgumentException e) when(e.InnerException is TypeLoadException)
            {
                _logger?.Log($"Cannot determine the default value for member '{memberInfo.DeclaringType.FullName}.{memberInfo.Name}', because type '{memberInfo.DeclaringType.FullName}' has generic parameters with base class or interface constraints.", LogLevel.Debug);
            }
            catch (Exception e)
            {
                _logger?.Log($"Cannot determine the default value for member '{memberInfo.DeclaringType.FullName}.{memberInfo.Name}', because an unknown exception occurred: '{e.Message}'", LogLevel.Debug);
            }

            return null;
        }

        private bool memberwiseEquals(object a, object b)
        {
            if (a == b || a.Equals(b)) return true;
            if (a.GetType() != b.GetType()) return false;
            var type = a.GetType();
            if (type.GetTypeInfo().IsValueType)
            {
                return false;
            }
            while (type != null)
            {
                foreach (var member in type.GetTsExportableMembers(this._metadataReaderFactory.GetInstance()))
                {
                    if (member is PropertyInfo p && p.GetIndexParameters().Length > 0) continue;
                    GetMemberValue(member, a, out var aVal, out var _);
                    GetMemberValue(member, b, out var bVal, out var _);
                    if (!memberwiseEquals(aVal, bVal))
                    {
                        _logger?.Log($"Difference found in member {type.FullName}.{member.Name}: {aVal} != {bVal}", LogLevel.Info);
                        return false;
                    }
                }
                type = type.GetTypeInfo().BaseType;
            }
            return true;
        }

        private void GetMemberValue(MemberInfo memberInfo, object instance, out object valueObj, out bool isConstant)
        {
            switch (memberInfo)
            {
                case FieldInfo fieldInfo:
                    valueObj = fieldInfo.GetValue(instance);
                    isConstant = fieldInfo.IsStatic && fieldInfo.IsLiteral && !fieldInfo.IsInitOnly;
                    break;
                case PropertyInfo propertyInfo:
                    valueObj = propertyInfo.GetValue(instance);
                    isConstant = false;
                    break;
                default:
                    throw new Exception();
            }

        }

        public string GetConstructorText(Type type)
        {
            var reader = _metadataReaderFactory.GetInstance();
            var ctorParams = GetHierarchyConstructorParams(type);
            if (ctorParams.Count == 0 || ctorParams.All(l => l.Count == 0))
            {
                return "";
            }
            ctorParams.Reverse();
            var paramsText = string.Join(", ", ctorParams.SelectMany(_ => _).Select(m =>
            {
                string defaultValue;
                var typeName = _typeService.GetTsTypeName(m);
                // try to get default value from TsDefaultValueAttribute
                var defaultValueAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsDefaultValueAttribute>(m);
                if (defaultValueAttribute != null)
                {
                    defaultValue = defaultValueAttribute.DefaultValue;
                }
                else
                {
                    // try to get default value from the member's default value
                    string valueText = GetMemberValueText(m);
                    if (!string.IsNullOrWhiteSpace(valueText))
                        defaultValue = valueText;
                    // try to get default value from Options.DefaultValuesForTypes
                    else if (_generatorOptionsProvider.GeneratorOptions.DefaultValuesForTypes.Any() && _generatorOptionsProvider.GeneratorOptions.DefaultValuesForTypes.ContainsKey(typeName))
                        defaultValue = _generatorOptionsProvider.GeneratorOptions.DefaultValuesForTypes[typeName];
                    else
                        defaultValue = null;
                }
                var ret = $"{GetTsMemberName(m)}: {typeName}";
                if (!string.IsNullOrWhiteSpace(defaultValue))
                {
                    ret += $" = {defaultValue}";
                }
                return ret;
            }));
            string tab = GeneratorOptions.UseTabCharacter ? "\t" : StringUtils.GetTabText(GeneratorOptions.TabLength);
            var newParams = ctorParams.Last();
            var superParams = new List<List<MemberInfo>>(ctorParams);
            superParams.RemoveAt(superParams.Count - 1);
            var superText = superParams.Count == 0 || superParams.All(l => l.Count == 0) ? "" : $"{tab}{tab}super({string.Join(", ", superParams.SelectMany(_ => _).Select(GetTsMemberName))});\r\n";
            var bodyText = newParams.Aggregate("", (acc, m) => acc + _templateService.FillConstructorAssignmentTemplate(GetTsMemberName(m)));
            return _templateService.FillConstructorTemplate(_typeService.GetTsTypeName(type), paramsText, superText, bodyText);
        }

        private List<List<MemberInfo>> GetHierarchyConstructorParams(Type type)
        {
            var reader = _metadataReaderFactory.GetInstance();
            var ret = new List<List<MemberInfo>>();
            while (type != null)
            {
                var parameters = type.GetTsExportableMembers(reader).Where(m => reader.GetAttribute<TsConstructorAttribute>(m) != null).ToList();
                ret.Add(parameters);
                type = type.GetTypeInfo().BaseType;
            }
            return ret;
        }

        private string GetTsMemberName(MemberInfo memberInfo)
        {
            var nameAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsMemberNameAttribute>(memberInfo);
            return nameAttribute?.Name ?? _generatorOptionsProvider.GeneratorOptions.PropertyNameConverters.Convert(memberInfo.Name, memberInfo);
        }
    }
}
