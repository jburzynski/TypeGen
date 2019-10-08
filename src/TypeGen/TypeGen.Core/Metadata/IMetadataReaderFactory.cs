using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;

namespace TypeGen.Core.Metadata
{
    internal interface IMetadataReaderFactory
    {
        IMetadataReader GetInstance();
    }
}