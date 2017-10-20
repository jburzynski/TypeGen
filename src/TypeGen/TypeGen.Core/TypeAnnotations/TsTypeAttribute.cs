using System;
using System.Linq;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Specifies the generated TypeScript type for a property or field
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsTypeAttribute : Attribute
    {
        /// <summary>
        /// The TypeScript property type name
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Type name without special characters ([], &lt;&gt;)
        /// </summary>
        public string FlatTypeName => TypeName?.Split('[', '<').First();

        /// <summary>
        /// The path of the file to import (can be left null if no imports are required)
        /// </summary>
        public string ImportPath { get; set; }

        /// <summary>
        /// The original TypeScript type name.
        /// This property should be used when the specified TypeName differs from the original type name defined in the file under ImportPath.
        /// This property should only be used in conjunction with ImportPath.
        /// </summary>
        public string OriginalTypeName { get; set; }

        public TsTypeAttribute(TsType type)
        {
            switch (type)
            {
                case TsType.Object:
                    TypeName = "Object";
                    break;
                case TsType.Boolean:
                    TypeName = "boolean";
                    break;
                case TsType.String:
                    TypeName = "string";
                    break;
                case TsType.Number:
                    TypeName = "number";
                    break;
                case TsType.Date:
                    TypeName = "Date";
                    break;
                case TsType.Any:
                    TypeName = "any";
                    break;
            }
        }

        /// <summary>
        /// TsTypeAttribute constructor
        /// </summary>
        /// <param name="typeName">The TypeScript property type name</param>
        /// <param name="importPath">The path of the file to import (optional)</param>
        /// <param name="originalTypeName">The original TypeScript type name, defined in the file under ImportPath (optional)</param>
        public TsTypeAttribute(string typeName, string importPath = null, string originalTypeName = null)
        {
            TypeName = typeName;
            ImportPath = importPath;
            OriginalTypeName = originalTypeName;
        }
    }
}
