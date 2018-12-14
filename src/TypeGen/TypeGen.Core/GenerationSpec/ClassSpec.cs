using System;
using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.GenerationSpec
{
    internal class ClassSpec
    {
        public ExportTsClassAttribute ExportAttribute { get; set; }
        public IList<Attribute> AdditionalAttributes { get; }

        public IDictionary<string, IList<Attribute>> MemberAttributes { get; }

        public ClassSpec()
        {
            MemberAttributes = new Dictionary<string, IList<Attribute>>();
            AdditionalAttributes = new List<Attribute>();
        }

        public void AddMember(string memberName)
        {
            MemberAttributes[memberName] = new List<Attribute>();
        }
    }
}