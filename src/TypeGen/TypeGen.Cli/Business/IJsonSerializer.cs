namespace TypeGen.Cli.Business
{
    internal interface IJsonSerializer
    {
        TObj DeserializeFromFile<TObj>(string filePath) where TObj : class;
        TObj Deserialize<TObj>(string jsonString) where TObj : class;
    }
}