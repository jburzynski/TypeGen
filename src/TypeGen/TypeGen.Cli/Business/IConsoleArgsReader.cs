using System.Collections.Generic;

namespace TypeGen.Cli.Business
{
    internal interface IConsoleArgsReader
    {
        bool ContainsHelpParam(string[] args);
        bool ContainsGetCwdParam(string[] args);
        bool ContainsVerboseParam(string[] args);
        IEnumerable<string> GetConfigPaths(string[] args);
        IEnumerable<string> GetProjectFolders(string[] args);
    }
}