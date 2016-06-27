using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TypeGen.Core.Converters;
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

        public Generator(string baseDirectory = "")
        {
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

            string tsClassName = Options.TypeNameConverters.Convert(type.Name, type);
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
            string name = Options.PropertyNameConverters.Convert(propertyInfo.Name);

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
            return template.Replace("$tg{tab}", GetTabText());
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
            string fileName = Options.FileNameConverters.Convert(type.Name);

            if (!string.IsNullOrEmpty(Options.TypeScriptFileExtension))
            {
                fileName += "." + Options.TypeScriptFileExtension;
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

        /// <summary>
        /// Gets a string value to use as a tab
        /// </summary>
        /// <returns></returns>
        private string GetTabText()
        {
            string tabText = string.Empty;
            for (int i = 0; i < Options.TabLength; i++)
            {
                tabText += " ";
            }
            return tabText;
        }
    }
}
