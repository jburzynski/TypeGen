using TypeGen.Cli.Models;

namespace TypeGen.Cli.Business
{
    internal interface IConfigProvider
    {
        /// <summary>
        /// Creates a config object from a given config file
        /// </summary>
        /// <param name="configPath"></param>
        /// <param name="projectFolder"></param>
        /// <returns></returns>
        TgConfig GetConfig(string configPath, string projectFolder);
    }
}