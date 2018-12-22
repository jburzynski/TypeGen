using System;
using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    internal class TypeSpec
    {
        public ExportAttribute ExportAttribute { get; }
        public IList<Attribute> AdditionalAttributes { get; }
        
        public IDictionary<string, IList<Attribute>> MemberAttributes { get; }
        
        public TypeSpec(ExportAttribute exportAttribute)
        {
            ExportAttribute = exportAttribute;
            MemberAttributes = new Dictionary<string, IList<Attribute>>();
            AdditionalAttributes = new List<Attribute>();
        }
        
        public void AddMember(string memberName)
        {
            MemberAttributes[memberName] = new List<Attribute>();
        }
    }
}