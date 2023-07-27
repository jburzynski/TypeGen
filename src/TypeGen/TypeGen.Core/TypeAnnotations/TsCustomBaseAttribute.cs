using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Specifies custom base class declaration for a TypeScript class or interface.
    /// Base class declaration is empty if no content is specified.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct, Inherited = false)]
    public class TsCustomBaseAttribute : Attribute
    {
        /// <summary>
        /// Base class/interface type name.
        /// </summary>
        public string Base { get; set; }

        /// <summary>
        /// The path of custom base type file to import (can be left null if no imports are required).
        /// </summary>
        public string ImportPath { get; set; }

        /// <summary>
        /// The original TypeScript base type name.
        /// This property should be used when the specified Base differs from the original base type name defined in the file under ImportPath.
        /// This property should only be used in conjunction with ImportPath.
        /// </summary>
        public string OriginalTypeName { get; set; }
        
        /// <summary>
        /// Whether default export is used for the referenced TypeScript type - used only in combination with ImportPath.
        /// </summary>
        public bool IsDefaultExport { get; set; }
        
        /// <summary>
        /// The implemented interfaces.
        /// </summary>
        public IEnumerable<ImplementedInterface> ImplementedInterfaces { get; set; } = Enumerable.Empty<ImplementedInterface>();

        public TsCustomBaseAttribute()
        {
        }

        /// <summary>
        /// TsCustomBaseAttribute constructor.
        /// </summary>
        /// <param name="base">The base type name (or alias)</param>
        public TsCustomBaseAttribute(string @base)
        {
            Base = @base;
        }

        /// <summary>
        /// TsCustomBaseAttribute constructor.
        /// </summary>
        /// <param name="base">The base type name (or alias).</param>
        /// <param name="importPath">The path of base type file to import (can be left null if no imports are required).</param>
        /// <param name="originalTypeName">The original TypeScript type name, defined in the file under <see cref="importPath"/> - used only if type alias is specified.</param>
        /// <param name="isDefaultExport">Whether default export is used for the referenced TypeScript type - used only in combination with <see cref="importPath"/>.</param>
        /// <param name="implementedInterfaces">The implemented interfaces.
        /// Values should be passed in the same order as in the <see cref="ImplementedInterface"/> record constructor.
        /// For example, to define two interfaces (IFoo and IBar),  
        /// the following arguments can be passed: "IFoo", null, null, false, "IBar", null, null, false.
        /// </param>
        public TsCustomBaseAttribute(string @base, string importPath = null, string originalTypeName = null, bool isDefaultExport = false,
            params object[] implementedInterfaces)
        {
            Base = @base;
            ImportPath = importPath;
            OriginalTypeName = originalTypeName;
            IsDefaultExport = isDefaultExport;
            
            ImplementedInterfaces = implementedInterfaces
                .Select((x, idx) => new { x, idx })
                .GroupBy(g => g.idx / 4)
                .Select(g =>
                {
                    var groupArray = g.ToArray();
                    return new ImplementedInterface(
                        (string)groupArray[0].x,
                        (string)groupArray[1].x,
                        (string)groupArray[2].x,
                        (bool)groupArray[3].x);
                })
                .ToList();
        }
    }
}
