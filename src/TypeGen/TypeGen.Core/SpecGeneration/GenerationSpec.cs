using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration.Builders;
using TypeGen.Core.SpecGeneration.Builders.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    /// <summary>
    /// Base class for generation specs
    /// </summary>
    public abstract class GenerationSpec
    {
        internal IDictionary<Type, TypeSpec> TypeSpecs { get; }
        internal IList<BarrelSpec> BarrelSpecs { get; }

        protected GenerationSpec()
        {
            TypeSpecs = new Dictionary<Type, TypeSpec>();
            BarrelSpecs = new List<BarrelSpec>();
        }

        /// <summary>
        /// The callback invoked before any files from the current generation spec are generated.
        /// </summary>
        /// <param name="args">The callback arguments.</param>
        public virtual void OnBeforeGeneration(OnBeforeGenerationArgs args)
        {
        }

        /// <summary>
        /// The callback invoked after the translated TypeScript files are generated from the current generation spec,
        /// but before any barrel files are generated for the current generation spec.
        /// </summary>
        /// <param name="args">The callback arguments.</param>
        public virtual void OnBeforeBarrelGeneration(OnBeforeBarrelGenerationArgs args)
        {
        }
        
        /// <summary>
        /// The callback invoked after all files from the current generation spec are generated.
        /// </summary>
        /// <param name="args">The callback arguments.</param>
        public virtual void OnAfterGeneration(OnAfterGenerationArgs args)
        {
        }

        /// <summary>
        /// Adds a class
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
        /// Adds a class
        /// </summary>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        protected ClassSpecBuilder<T> AddClass<T>(string outputDir = null)
        {
            TypeSpec typeSpec = AddTypeSpec(typeof(T), new ExportTsClassAttribute { OutputDir = outputDir });
            return new ClassSpecBuilder<T>(typeSpec);
        }
        
        /// <summary>
        /// Adds an interface
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
        /// Adds an interface
        /// </summary>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        protected InterfaceSpecBuilder<T> AddInterface<T>(string outputDir = null)
        {
            TypeSpec typeSpec = AddTypeSpec(typeof(T), new ExportTsInterfaceAttribute { OutputDir = outputDir });
            return new InterfaceSpecBuilder<T>(typeSpec);
        }

        /// <summary>
        /// Adds an enum
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputDir"></param>
        /// <param name="isConst"></param>
        /// <param name="asUnionType"></param>
        /// <returns></returns>
        protected EnumSpecBuilder AddEnum(Type type, string outputDir = null, bool isConst = false, bool asUnionType = false)
        {
            TypeSpec typeSpec = AddTypeSpec(type, new ExportTsEnumAttribute { OutputDir = outputDir, IsConst = isConst, AsUnionType = asUnionType });
            return new EnumSpecBuilder(typeSpec);
        }

        /// <summary>
        /// Adds an enum
        /// </summary>
        /// <param name="outputDir"></param>
        /// <param name="isConst"></param>
        /// <param name="asUnionType"></param>
        /// <returns></returns>
        protected EnumSpecBuilder<T> AddEnum<T>(string outputDir = null, bool isConst = false, bool asUnionType = false) where T : Enum
        {
            TypeSpec typeSpec = AddTypeSpec(typeof(T), new ExportTsEnumAttribute { OutputDir = outputDir, IsConst = isConst, AsUnionType = asUnionType });
            return new EnumSpecBuilder<T>(typeSpec);
        }

        private TypeSpec AddTypeSpec(Type type, ExportAttribute exportAttribute)
        {
            var typeSpec = new TypeSpec(exportAttribute);
            TypeSpecs[type] = typeSpec;

            return typeSpec;
        }

        /// <summary>
        /// Adds a barrel file for a specified directory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="barrelScope"></param>
        protected void AddBarrel(string directory, BarrelScope barrelScope = BarrelScope.Files | BarrelScope.Directories) => BarrelSpecs.Add(new BarrelSpec(directory, barrelScope));
    }
}