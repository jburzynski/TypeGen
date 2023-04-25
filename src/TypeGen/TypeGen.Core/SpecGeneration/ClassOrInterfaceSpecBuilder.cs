using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    /// <summary>
    /// Base class for class and interface spec builders
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDerived"></typeparam>
    public abstract class ClassOrInterfaceSpecBuilder<T, TDerived> : ClassOrInterfaceOrStructSpecBuilder<T, TDerived> where TDerived : ClassOrInterfaceSpecBuilder<T, TDerived>
    {
        internal ClassOrInterfaceSpecBuilder(TypeSpec spec) : base(spec)
        {
        }
        
        /// <summary>
        /// Specifies custom base for the type (equivalent of TsCustomBaseAttribute)
        /// </summary>
        /// <param name="base"></param>
        /// <param name="importPath"></param>
        /// <param name="originalTypeName"></param>
        /// <param name="isDefaultExport"></param>
        /// <returns></returns>
        public TDerived CustomBase(string @base = null, string importPath = null, string originalTypeName = null, bool isDefaultExport = false)
        {
            AddTypeAttribute(new TsCustomBaseAttribute(@base, importPath, originalTypeName, isDefaultExport));
            return this as TDerived;
        }

    }
}