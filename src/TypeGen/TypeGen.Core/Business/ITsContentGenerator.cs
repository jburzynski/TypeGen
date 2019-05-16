using System;
using System.Reflection;
using TypeGen.Core.Converters;

namespace TypeGen.Core.Business
{
    internal interface ITsContentGenerator
    {
        /// <summary>
        /// Gets code for the 'imports' section for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        /// <param name="fileNameConverters"></param>
        /// <param name="typeNameConverters"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when one of: type, fileNameConverters or typeNameConverters is null</exception>
        string GetImportsText(Type type, string outputDir, TypeNameConverterCollection fileNameConverters, TypeNameConverterCollection typeNameConverters);

        /// <summary>
        /// Gets the text for the "extends" section
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeNameConverters"></param>
        /// <returns></returns>
        string GetExtendsText(Type type, TypeNameConverterCollection typeNameConverters);

        /// <summary>
        /// Gets custom code for a TypeScript file given by filePath.
        /// Returns an empty string if a file does not exist.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="indentSize"></param>
        /// <returns></returns>
        string GetCustomBody(string filePath, int indentSize);

        /// <summary>
        /// Gets custom code for a TypeScript file given by filePath.
        /// Returns an empty string if a file does not exist.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string GetCustomHead(string filePath);

        GeneratorOptions GeneratorOptions { get; set; }

        /// <summary>
        /// Gets text to be used as a member value
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns>The text to be used as a member value. Null if the member has no value or value cannot be determined.</returns>
        string GetMemberValueText(MemberInfo memberInfo);
    }
}