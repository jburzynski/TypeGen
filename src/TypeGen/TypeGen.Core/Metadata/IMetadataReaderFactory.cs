using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;

namespace TypeGen.Core.Metadata
{
    internal interface IMetadataReaderFactory
    {
        GenerationType GenerationType { get; set; }
        GenerationSpec GenerationSpec { get; set; }
        IMetadataReader GetInstance();
    }
}