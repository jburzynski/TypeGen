using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration.Generic
{

    /// <summary>
    /// Builds the struct configuration section inside generation spec
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StructSpecBuilder<T> : ClassOrInterfaceOrStructSpecBuilder<T, StructSpecBuilder<T>>
    {
        internal StructSpecBuilder(TypeSpec spec) : base(spec) { }

        public StructSpecBuilder<T> Member(Func<T, string> memberNameFunc)
        {
            return Member(memberNameFunc(_instance));
        }

        /// <summary>
        /// Marks selected member as static (equivalent of TsStaticAttribute)
        /// </summary>
        /// <returns></returns>
        public StructSpecBuilder<T> Static()
        {
            AddActiveMemberAttribute(new TsStaticAttribute());
            return this;
        }

        /// <summary>
        /// Marks selected member as not static (equivalent of TsNotStaticAttribute)
        /// </summary>
        /// <returns></returns>
        public StructSpecBuilder<T> NotStatic()
        {
            AddActiveMemberAttribute(new TsNotStaticAttribute());
            return this;
        }
    }
}