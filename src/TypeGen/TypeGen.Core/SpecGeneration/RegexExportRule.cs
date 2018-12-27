using System;
using System.Collections.Generic;
using System.Linq;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.SpecGeneration
{
    internal class RegexExportRule
    {
        public ExportAttribute ExportAttribute { get; }
        public IList<Attribute> AdditionalAttributes { get; }

        public RegexExportRule(ExportAttribute exportAttribute, params Attribute[] additionalAttributes)
        {
            ExportAttribute = exportAttribute;
            AdditionalAttributes = additionalAttributes.ToList();
        }

        public void AddAdditionalAttribute(Attribute attribute)
        {
            AdditionalAttributes.Add(attribute);
        }
    }
}