using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.GenerationSpec
{
    public abstract class GenerationSpec
    {
        internal IDictionary<Type, TypeSpec> TypeSpecs { get; }

        private GenerationSpec()
        {
            TypeSpecs = new Dictionary<Type, TypeSpec>();
        }
        
        protected ClassSpecBuilder<T> AddClass<T>(string outputDir = null) where T : class
        {
            var typeSpec = new TypeSpec(new ExportTsClassAttribute { OutputDir = outputDir });
            TypeSpecs[typeof(T)] = typeSpec;
            
            return new ClassSpecBuilder<T>(typeSpec);
        }
        
        protected InterfaceSpecBuilder<T> AddInterface<T>(string outputDir = null) where T : class
        {
            var typeSpec = new TypeSpec(new ExportTsInterfaceAttribute { OutputDir = outputDir });
            TypeSpecs[typeof(T)] = typeSpec;
            
            return new InterfaceSpecBuilder<T>(typeSpec);
        }

        protected EnumSpecBuilder<T> AddEnum<T>(string outputDir = null, bool isConst = false) where T : Enum
        {
            var typeSpec = new TypeSpec(new ExportTsEnumAttribute { OutputDir = outputDir, IsConst = isConst });
            TypeSpecs[typeof(T)] = typeSpec;
            
            return new EnumSpecBuilder<T>(typeSpec);
        }
    }
}