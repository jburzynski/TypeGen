using System.Collections.Generic;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Metadata
{
    internal class MetadataReaderFactory : IMetadataReaderFactory
    {
        private IMetadataReader _instance;
        private GenerationSpec _previousGenerationSpec;

        private GenerationSpec _generationSpec;
        public GenerationSpec GenerationSpec
        {
            get => _generationSpec;
            set
            {
                _previousGenerationSpec = _generationSpec;
                _generationSpec = value;
            }
        }

        public IMetadataReader GetInstance()
        {
            Requires.NotNull(GenerationSpec, nameof(GenerationSpec));

            if (_previousGenerationSpec == GenerationSpec) return _instance;

            _instance = new GenerationSpecMetadataReader(GenerationSpec);
            _previousGenerationSpec = GenerationSpec;
            
            return _instance;
        }
    }
}