namespace TypeGen.Core.Storage
{
    internal interface IInternalStorage
    {
        /// <summary>
        /// Gets embedded resource as string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetEmbeddedResource(string name);
    }
}