namespace TypeGen.Cli.GenerationConfig
{
    internal interface IConfigProvider
    {
        /// <summary>
        /// Creates a config object from a given config file
        /// </summary>
        /// <param name="configPath"></param>
        /// <param name="projectFolder"></param>
        /// <param name="consoleOptions"></param>
        /// <returns></returns>
        TgConfig GetConfig(string configPath, string projectFolder, ConfigConsoleOptions consoleOptions);
    }
}