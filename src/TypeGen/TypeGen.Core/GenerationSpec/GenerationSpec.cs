using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.GenerationSpec
{
    public abstract class GenerationSpec
    {
        internal IDictionary<Type, ClassSpec> ClassSpecs { get; }

        private GenerationSpec()
        {
            ClassSpecs = new Dictionary<Type, ClassSpec>();
        }

        protected ClassSpecBuilder<T> AddClass<T>(string outputDir = null)
        {
            var classSpec = new ClassSpec { ExportAttribute = new ExportTsClassAttribute { OutputDir = outputDir } };
            ClassSpecs[typeof(T)] = classSpec;
            
            return new ClassSpecBuilder<T>(classSpec);
        }
    }
}