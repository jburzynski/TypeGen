using System.Collections.Generic;

namespace TypeGen.Cli.Business
{
    internal interface IConsoleArgsReader
    {
        bool ContainsGetCwdCommand(string[] args);
        bool ContainsGenerateCommand(string[] args);
        bool ContainsAnyCommand(string[] args);
        bool ContainsHelpOption(string[] args);
        bool ContainsProjectFolderOption(string[] args);
        bool ContainsVerboseOption(string[] args);
        IEnumerable<string> GetProjectFolders(string[] args);
        IEnumerable<string> GetConfigPaths(string[] args);
    }
}