using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Core
{
    /// <summary>
    /// Class used for generating TypeScript files from C# files
    /// </summary>
    public class Generator
    {
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
        /// Determines file extension used for the generated TypeScript files. Default is "ts".
        /// </summary>
        public string TypeScriptFileExtension { get; set; }

        public Generator()
        {
            FileNameConverter = new PascalCaseToKebabCaseConverter();
            TypeNameConverter = new NoChangeConverter();
            PropertyNameConverter = new PascalCaseToCamelCaseConverter();
            TypeScriptFileExtension = "ts";
        }
    }
}
