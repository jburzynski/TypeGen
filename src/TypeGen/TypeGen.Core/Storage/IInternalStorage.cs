namespace TypeGen.Core.Storage
{
    public interface IInternalStorage
    {
        /// <summary>
        /// Gets embedded resource as string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetEmbeddedResource(string name);
    }
}