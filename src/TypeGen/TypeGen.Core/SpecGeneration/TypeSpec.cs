using System;
using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    internal class TypeSpec
    {
        private readonly List<Attribute> _additionalAttributes;
        private readonly Dictionary<string, IList<Attribute>> _memberAttributes;

        public ExportAttribute ExportAttribute { get; }

        public IList<Attribute> AdditionalAttributes => _additionalAttributes;

        public IDictionary<string, IList<Attribute>> MemberAttributes => _memberAttributes;

        public TypeSpec(ExportAttribute exportAttribute)
        {
            ExportAttribute = exportAttribute;
            _memberAttributes = new Dictionary<string, IList<Attribute>>();
            _additionalAttributes = new List<Attribute>();
        }
        
        public void AddMember(string memberName) => _memberAttributes[memberName] = new List<Attribute>();

        public void AddCustomBaseAttribute(string @base = null, string importPath = null, string originalTypeName = null, bool isDefaultExport = false)
            => AddAdditionalAttribute(new TsCustomBaseAttribute(@base, importPath, originalTypeName, isDefaultExport));
        public void AddDefaultExportAttribute(bool enabled = true) => AddAdditionalAttribute(new TsDefaultExportAttribute(enabled));
        public void AddDefaultTypeOutputAttribute(string memberName, string outputDir) => AddMemberAttribute(memberName, new TsDefaultTypeOutputAttribute(outputDir));
        public void AddDefaultValueAttribute(string memberName, string defaultValue) => AddMemberAttribute(memberName, new TsDefaultValueAttribute(defaultValue));
        public void AddIgnoreAttribute(string memberName) => AddMemberAttribute(memberName, new TsIgnoreAttribute());
        public void AddIgnoreBaseAttribute() => AddAdditionalAttribute(new TsIgnoreBaseAttribute());
        public void AddMemberNameAttribute(string memberName, string name) => AddMemberAttribute(memberName, new TsMemberNameAttribute(name));
        public void AddNotNullAttribute(string memberName) => AddMemberAttribute(memberName, new TsNotNullAttribute());
        public void AddNotReadonlyAttribute(string memberName) => AddMemberAttribute(memberName, new TsNotReadonlyAttribute());
        public void AddNotStaticAttribute(string memberName) => AddMemberAttribute(memberName, new TsNotStaticAttribute());
        public void AddNotUndefinedAttribute(string memberName) => AddMemberAttribute(memberName, new TsNotUndefinedAttribute());
        public void AddNullAttribute(string memberName) => AddMemberAttribute(memberName, new TsNullAttribute());
        public void AddOptionalAttribute(string memberName) => AddMemberAttribute(memberName, new TsOptionalAttribute());
        public void AddReadonlyAttribute(string memberName) => AddMemberAttribute(memberName, new TsReadonlyAttribute());
        public void AddStaticAttribute(string memberName) => AddMemberAttribute(memberName, new TsStaticAttribute());
        public void AddStringInitializersAttribute(bool enabled = true) => AddAdditionalAttribute(new TsStringInitializersAttribute(enabled));
        public void AddTypeAttribute(string memberName, TsType tsType) => AddMemberAttribute(memberName, new TsTypeAttribute(tsType));
        public void AddTypeAttribute(string memberName, string typeName, string importPath = null, string originalTypeName = null, bool isDefaultExport = false)
            => AddMemberAttribute(memberName, new TsTypeAttribute(typeName, importPath, originalTypeName, isDefaultExport));
        public void AddTypeUnionsAttribute(string memberName, IEnumerable<string> typeUnions) => AddMemberAttribute(memberName, new TsTypeUnionsAttribute(typeUnions));
        public void AddTypeUnionsAttribute(string memberName, params string[] typeUnions) => AddMemberAttribute(memberName, new TsTypeUnionsAttribute(typeUnions));
        public void AddUndefinedAttribute(string memberName) => AddMemberAttribute(memberName, new TsUndefinedAttribute());
        
        private void AddMemberAttribute(string memberName, Attribute attribute) => MemberAttributes[memberName].Add(attribute);
        private void AddAdditionalAttribute(Attribute attribute) => _additionalAttributes.Add(attribute);
    }
}