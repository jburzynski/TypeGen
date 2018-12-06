using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.Extensions;

namespace TypeGen.Cli.Business
{
    internal class ConsoleArgsReader : IConsoleArgsReader
    {
        private const string GetCwdCommand = "GETCWD";
        private const string GenerateCommand = "GENERATE";

        /// <summary>
        /// Used to separate two or more paths; not a directory separator
        /// </summary>
        private const string PathSeparator = "|";

        public bool ContainsGetCwdCommand(string[] args) => ContainsCommand(args, GetCwdCommand);
        public bool ContainsGenerateCommand(string[] args) => ContainsCommand(args, GenerateCommand);
        public bool ContainsAnyCommand(string[] args) => ContainsGenerateCommand(args) || ContainsGetCwdCommand(args);
        private bool ContainsCommand(string[] args, string command) => args.Any(arg => string.Equals(arg, command, StringComparison.InvariantCultureIgnoreCase));

        public bool ContainsHelpOption(string[] args) => ContainsOption(args, "-h", "--help");
        public bool ContainsProjectFolderOption(string[] args) => ContainsOption(args, "-p", "--project-folder");
        public bool ContainsVerboseOption(string[] args) => ContainsOption(args, "-v", "--verbose");
        private bool ContainsOption(string[] args, string optionShortName, string optionFullName) => args.Any(arg => string.Equals(arg, optionShortName, StringComparison.InvariantCultureIgnoreCase) || string.Equals(arg, optionFullName, StringComparison.InvariantCultureIgnoreCase));

        public IEnumerable<string> GetProjectFolders(string[] args) => GetPathsParam(args, "-p", "--project-folder");
        public IEnumerable<string> GetConfigPaths(string[] args) => GetPathsParam(args, "-c", "--config-path");

        private IEnumerable<string> GetPathsParam(string[] args, string paramShortName, string paramFullName)
        {
            int index = -1;

            for (var i = 0; i < args.Length; i++)
            {
                if (args[i].Equals(paramShortName, StringComparison.InvariantCultureIgnoreCase) ||
                    args[i].Equals(paramFullName, StringComparison.InvariantCultureIgnoreCase))
                {
                    index = i;
                    break;
                }
            }

            if (index < 0) return Enumerable.Empty<string>();

            if (args.Length < index + 2) throw new CliException($"{paramShortName}|{paramFullName} parameter present, but no path specified");
            return args[index + 1].Split(PathSeparator);
        }
    }
}