using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.Extensions;
using TypeGen.Core.Metadata;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.TypeAnnotations;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Generator.Services
{
    internal class GenerationSpecProvider
    {
        public GenerationSpec GetGenerationSpec(IEnumerable<Assembly> assemblies)
        {
            Requires.NotNull(assemblies, nameof(assemblies));

            var metadataReader = new AttributeMetadataReader();
            var generationSpec = new GenerationSpecProviderGenerationSpec();

            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> types = assembly.GetLoadableTypes()
                    .GetExportMarkedTypes(metadataReader);

                foreach (Type type in types)
                {
                    TypeSpec typeSpec = GetTypeSpec(type, metadataReader);
                    generationSpec.TypeSpecs.Add(type, typeSpec);
                }
            }

            return generationSpec;
        }

        public GenerationSpec GetGenerationSpec(Type type)
        {
            Requires.NotNull(type, nameof(type));
            
            var metadataReader = new AttributeMetadataReader();
            var generationSpec = new GenerationSpecProviderGenerationSpec();

            TypeSpec typeSpec = GetTypeSpec(type, metadataReader);
            generationSpec.TypeSpecs.Add(type, typeSpec);

            return generationSpec;
        }

        private TypeSpec GetTypeSpec(Type type, IMetadataReader metadataReader)
        {
            if (!type.HasExportAttribute(metadataReader))
                throw new CoreException($"Type '{type.FullName}' should have an ExportAttribute attribute");

            var exportAttribute = metadataReader.GetAttributes(type).FirstOrDefault(a => a is ExportAttribute) as ExportAttribute;

            IEnumerable<Attribute> additionalAttributes = metadataReader.GetAttributes(type)
                .Where(a => !(a is ExportAttribute))
                .OfType<Attribute>();
            
            var typeSpec = new TypeSpec(exportAttribute);
            
            foreach (Attribute additionalAttribute in additionalAttributes)
            {
                typeSpec.AdditionalAttributes.Add(additionalAttribute);
            }
            
            foreach (MemberInfo memberInfo in type.GetTsExportableMembers(metadataReader, false))
            {
                IEnumerable<Attribute> attributes = metadataReader
                    .GetAttributes(memberInfo)
                    .OfType<Attribute>();
                typeSpec.MemberAttributes.Add(memberInfo.Name, attributes.ToList());
            }

            return typeSpec;
        }

        public class GenerationSpecProviderGenerationSpec : GenerationSpec
        {
        }
    }
}