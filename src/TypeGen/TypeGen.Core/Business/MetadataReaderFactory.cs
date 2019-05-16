using System.Collections;
using System.Collections.Generic;
using TypeGen.Core.SpecGeneration;

namespace TypeGen.Core.Business
{
    internal class MetadataReaderFactory : IMetadataReaderFactory
    {
        private readonly IDictionary<GenerationType, IMetadataReader> _instances = new Dictionary<GenerationType, IMetadataReader>();

        public GenerationType GenerationType { get; set; }
        public GenerationSpec GenerationSpec { get; set; }
        
        public IMetadataReader GetInstance()
        {
            return _instances.ContainsKey(GenerationType) ? _instances[GenerationType] : CreateInstance();
        }

        private IMetadataReader CreateInstance()
        {
            IMetadataReader instance;
            
            switch (GenerationType)
            {
                case GenerationType.GenerationSpec:
                    instance = new GenerationSpecMetadataReader(GenerationSpec);
                    break;
                case GenerationType.GenerationSpecWithAttributes:
                    instance = new ComboMetadataReader(new GenerationSpecMetadataReader(GenerationSpec), new AttributeMetadataReader());
                    break;
                default:
                    instance = new AttributeMetadataReader();
                    break;
            }

            _instances[GenerationType] = instance;
            return instance;
        }
    }
}