using TypeGen.Core.SpecGeneration;

namespace TypeGen.Core.Business
{
    internal interface IMetadataReaderFactory
    {
        GenerationType GenerationType { get; set; }
        GenerationSpec GenerationSpec { get; set; }
        IMetadataReader GetInstance();
    }
}