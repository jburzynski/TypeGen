using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TypeGen.Types;

namespace TypeGen.Core
{
    /// <summary>
    /// Class used for generating TypeScript files from C# files
    /// </summary>
    public class Generator
    {
        private readonly string _classTemplate;
        private readonly string _classPropertyTemplate;
        private readonly string _classPropertyWithDefaultValueTemplate;
        private readonly string _baseDirectory;
        private string _tabText;

        /// <summary>
        /// Converter used for converting C# file names to TypeScript file names
        /// </summary>
        public INameConverter FileNameConverter { get; set; }

        /// <summary>
        /// Converter used for converting C# type names (classes, enums etc.) to TypeScript type names
        /// </summary>
        public ITypeNameConverter TypeNameConverter { get; set; }

        /// <summary>
        /// Converter used for converting C# property names to TypeScript property names
        /// </summary>
        public INameConverter PropertyNameConverter { get; set; }

        /// <summary>
        /// File extension used for the generated TypeScript files. Default is "ts".
        /// </summary>
        public string TypeScriptFileExtension { get; set; }

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
                    throw new ArgumentNullException("Options");
                }
                _options = value;
            }
        }

        private int _tabLength;

        /// <summary>
        /// Number of space characters per tab. Default is 4.
        /// </summary>
        public int TabLength {
            get
            {
                return _tabLength;
            }
            set
            {
                _tabLength = value;
                _tabText = string.Empty;
                for (int i = 0; i < _tabLength; i++)
                {
                    _tabText += " ";
                }
            }
        }

        public Generator(string baseDirectory = "")
        {
            FileNameConverter = new PascalCaseToKebabCaseConverter();
            TypeNameConverter = new NoChangeConverter();
            PropertyNameConverter = new PascalCaseToCamelCaseConverter();
            TypeScriptFileExtension = "ts";
            TabLength = 4;
            Options = new GeneratorOptions();

            _baseDirectory = baseDirectory;

            // initialize templates
            _classTemplate = Utilities.GetEmbeddedResource("TypeGen.Core.Templates.Class.tpl");
            _classPropertyTemplate = Utilities.GetEmbeddedResource("TypeGen.Core.Templates.ClassProperty.tpl");
            _classPropertyWithDefaultValueTemplate = Utilities.GetEmbeddedResource("TypeGen.Core.Templates.ClassPropertyWithDefaultValue.tpl");
        }

        /// <summary>
        /// Generates TypeScript files for C# files in an assembly
        /// </summary>
        /// <param name="assembly"></param>
        public void GenerateFromAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                var classAttribute = type.GetCustomAttribute<TsClassAttribute>();
                if (classAttribute != null)
                {
                    GenerateClass(type, classAttribute);
                }

                var enumAttribute = type.GetCustomAttribute<TsEnumAttribute>();
                if (enumAttribute != null)
                {
                    //GenerateEnum(type, enumAttribute);
                }
            }
        }

        /// <summary>
        /// Generates a TypeScript class file from a class type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="classAttribute"></param>
        private void GenerateClass(Type type, TsClassAttribute classAttribute)
        {
            string propertiesText = string.Empty;
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            // create TypeScript source code for properties' definition

            propertiesText = propertyInfos.Aggregate(propertiesText,
                (current, propertyInfo) => current + GetPropertyText(propertyInfo));

            if (propertiesText != string.Empty)
            {
                // remove the last new line symbol
                propertiesText = propertiesText.Remove(propertiesText.Length - 2);
            }

            // create TypeScript source code for the whole class

            string tsClassName = TypeNameConverter.Convert(type.Name, type);
            string filePath = GetFilePath(type, classAttribute.OutputDir);

            string classText = FillClassTemplate(string.Empty, tsClassName, propertiesText);

            // write TypeScript file

            File.WriteAllText(_baseDirectory + "\\" + filePath, classText);
        }

        /// <summary>
        /// Gets TypeScript property definition source code
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private string GetPropertyText(PropertyInfo propertyInfo)
        {
            string accessorText = Options.ExplicitPublicAccessor ? "public " : string.Empty;
            string name = PropertyNameConverter.Convert(propertyInfo.Name);

            var defaultValueAttribute = propertyInfo.GetCustomAttribute<TsDefaultValueAttribute>();
            if (defaultValueAttribute != null)
            {
                return FillClassPropertyWithDefaultValueTemplate(accessorText, name, defaultValueAttribute.DefaultValue);
            }

            string type = Utilities.GetTsTypeName(propertyInfo.PropertyType);
            return FillClassPropertyTemplate(accessorText, name, type);
        }

        private string FillClassTemplate(string imports, string name, string properties)
        {
            return ReplaceTabs(_classTemplate)
                .Replace("$tg{imports}", imports)
                .Replace("$tg{name}", name)
                .Replace("$tg{properties}", properties);
        }

        private string FillClassPropertyWithDefaultValueTemplate(string accessor, string name, string defaultValue)
        {
            return ReplaceTabs(_classPropertyWithDefaultValueTemplate)
                .Replace("$tg{accessor}", accessor)
                .Replace("$tg{name}", name)
                .Replace("$tg{defaultValue}", defaultValue);
        }

        private string FillClassPropertyTemplate(string accessor, string name, string type)
        {
            return ReplaceTabs(_classPropertyTemplate)
                .Replace("$tg{accessor}", accessor)
                .Replace("$tg{name}", name)
                .Replace("$tg{type}", type);
        }

        private string ReplaceTabs(string template)
        {
            return template.Replace("$tg{tab}", _tabText);
        }

        /// <summary>
        /// Generates a TypeScript enum file from a class type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="enumAttribute"></param>
        private void GenerateEnum(Type type, TsEnumAttribute enumAttribute)
        {
            
        }

        /// <summary>
        /// Gets the output TypeScript file path based on a type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        private string GetFilePath(Type type, string outputDir)
        {
            string fileName = FileNameConverter.Convert(type.Name);

            if (!string.IsNullOrEmpty(TypeScriptFileExtension))
            {
                fileName += "." + TypeScriptFileExtension;
            }

            if (string.IsNullOrEmpty(outputDir))
            {
                return fileName;
            }

            outputDir = outputDir.Replace('/', '\\');
            if (outputDir.Last() == '\\')
            {
                outputDir = outputDir.Remove(outputDir.Length - 1);
            }

            return outputDir + "\\" + fileName;
        }
    }
}
