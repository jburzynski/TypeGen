using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            }
        }

        public TsTypeAttribute(string typeName)
        {
            TypeName = typeName;
        }
    }
}
