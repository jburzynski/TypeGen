namespace TypeGen.Core.Metadata
{
    internal interface IMetadataReaderFactory
    {
        IMetadataReader GetInstance();
    }
}