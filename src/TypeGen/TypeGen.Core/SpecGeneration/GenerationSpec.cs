using System;
using System.Collections.Generic;
using System.Reflection;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    /// <summary>
    /// Base class for generation specs
    /// </summary>
    public abstract class GenerationSpec
    {
        internal IDictionary<Type, TypeSpec> TypeSpecs { get; }

        protected GenerationSpec()
        {
            TypeSpecs = new Dictionary<Type, TypeSpec>();
        }

        /// <summary>
        /// Adds class configuration section
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        protected ClassSpecBuilder AddClass(Type type, string outputDir = null)
        {
            TypeSpec typeSpec = AddTypeSpec(type, new ExportTsClassAttribute { OutputDir = outputDir });
            return new ClassSpecBuilder(typeSpec);
        }
        
        /// <summary>
        /// Adds class configuration section
        /// </summary>
        /// <param name="outputDir"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected Generic.ClassSpecBuilder<T> AddClass<T>(string outputDir = null) where T : class
        {
            TypeSpec typeSpec = AddTypeSpec(typeof(T), new ExportTsClassAttribute { OutputDir = outputDir });
            return new Generic.ClassSpecBuilder<T>(typeSpec);
        }
        
        /// <summary>
        /// Adds interface configuration section
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        protected InterfaceSpecBuilder AddInterface(Type type, string outputDir = null)
        {
            TypeSpec typeSpec = AddTypeSpec(type, new ExportTsInterfaceAttribute { OutputDir = outputDir });
            return new InterfaceSpecBuilder(typeSpec);
        }
        
        /// <summary>
        /// Adds interface configuration section
        /// </summary>
        /// <param name="outputDir"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected Generic.InterfaceSpecBuilder<T> AddInterface<T>(string outputDir = null) where T : class
        {
            TypeSpec typeSpec = AddTypeSpec(typeof(T), new ExportTsInterfaceAttribute { OutputDir = outputDir });
            return new Generic.InterfaceSpecBuilder<T>(typeSpec);
        }

        /// <summary>
        /// Adds enum configuration section
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        /// <param name="isConst"></param>
        /// <returns></returns>
        protected EnumSpecBuilder AddEnum(Type type, string outputDir = null, bool isConst = false)
        {
            TypeSpec typeSpec = AddTypeSpec(type, new ExportTsEnumAttribute { OutputDir = outputDir, IsConst = isConst });
            return new EnumSpecBuilder(typeSpec);
        }

        /// <summary>
        /// Adds enum configuration section
        /// </summary>
        /// <param name="outputDir"></param>
        /// <param name="isConst"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected Generic.EnumSpecBuilder<T> AddEnum<T>(string outputDir = null, bool isConst = false) where T : Enum
        {
            TypeSpec typeSpec = AddTypeSpec(typeof(T), new ExportTsEnumAttribute { OutputDir = outputDir, IsConst = isConst });
            return new Generic.EnumSpecBuilder<T>(typeSpec);
        }

        private TypeSpec AddTypeSpec(Type type, ExportAttribute exportAttribute)
        {
            var typeSpec = new TypeSpec(exportAttribute);
            TypeSpecs[type] = typeSpec;

            return typeSpec;
        }
    }
}