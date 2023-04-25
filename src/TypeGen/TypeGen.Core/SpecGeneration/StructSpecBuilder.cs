using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{

    /// <summary>
    /// Builds the struct configuration section inside generation spec
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StructSpecBuilder : ClassOrInterfaceOrStructSpecBuilder<dynamic, StructSpecBuilder>
    {
        internal StructSpecBuilder(TypeSpec spec) : base(spec) { }

        /// <summary>
        /// Marks selected member as static (equivalent of TsStaticAttribute)
        /// </summary>
        /// <returns></returns>
        public StructSpecBuilder Static()
        {
            AddActiveMemberAttribute(new TsStaticAttribute());
            return this;
        }

        /// <summary>
        /// Marks selected member as not static (equivalent of TsNotStaticAttribute)
        /// </summary>
        /// <returns></returns>
        public StructSpecBuilder NotStatic()
        {
            AddActiveMemberAttribute(new TsNotStaticAttribute());
            return this;
        }
    }
}
