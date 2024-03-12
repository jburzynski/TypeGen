using TypeGen.Core.SpecGeneration;

namespace TypeGen.Core.Metadata
{
    public interface IMetadataReaderFactory
    {
        IMetadataReader GetInstance();
        GenerationSpec GenerationSpec { get; set; }
    }
}