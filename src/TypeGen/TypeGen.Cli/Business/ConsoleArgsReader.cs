using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.Extensions;

namespace TypeGen.Cli.Business
{
    internal class ConsoleArgsReader
    {
        private const string GetCwdCommand = "GETCWD";
        private const string GenerateCommand = "GENERATE";

        /// <summary>
        /// Used to separate two or more paths; not a directory separator
        /// </summary>
        private const string PathSeparator = "|";

        public static bool ContainsGetCwdCommand(string[] args) => ContainsCommand(args, GetCwdCommand);
        public static bool ContainsGenerateCommand(string[] args) => ContainsCommand(args, GenerateCommand);
        public static bool ContainsAnyCommand(string[] args) => ContainsGenerateCommand(args) || ContainsGetCwdCommand(args);
        private static bool ContainsCommand(string[] args, string command) => args.Any(arg => string.Equals(arg, command, StringComparison.InvariantCultureIgnoreCase));

        public static bool ContainsHelpOption(string[] args) => ContainsOption(args, "-h", "--help");
        public static bool ContainsProjectFolderOption(string[] args) => ContainsOption(args, "-p", "--project-folder");
        public static bool ContainsOutputOption(string[] args) => ContainsOption(args, "-o", "--output-folder");
        public static bool ContainsRecordClassOption(string[] args) => ContainsOption(args, "-er", "--exclude-record-class");
        public static bool ContainsIncludeBaseClassForInterfacesOption(string[] args) => ContainsOption(args, "-ib", "--include-base-for-interfaces");
        public static bool ContainsVerboseOption(string[] args) => ContainsOption(args, "-v", "--verbose");
        private static bool ContainsOption(string[] args, string optionShortName, string optionFullName) => args.Any(arg => string.Equals(arg, optionShortName, StringComparison.InvariantCultureIgnoreCase) || string.Equals(arg, optionFullName, StringComparison.InvariantCultureIgnoreCase));

        public static IEnumerable<string> GetProjectFolders(string[] args) => GetPathsParam(args, "-p", "--project-folder");
        public static string GetOutputFolder(string[] args) => GetPathsParam(args, "-o", "--output-folder").FirstOrDefault();
        public static IEnumerable<string> GetConfigPaths(string[] args) => GetPathsParam(args, "-c", "--config-path");
        public static bool? GetRecordClassOption(string[] args) => GetFlagParam(args, "-er", "--exclude-record-class");
        public static bool? GetIncludeBaseClassForInterfacesOption(string[] args) => GetFlagParam(args, "-ib", "--include-base-for-interfaces");

        private static IEnumerable<string> GetPathsParam(string[] args, string paramShortName, string paramFullName)
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

        private static bool? GetFlagParam(string[] args, string paramShortName, string paramFullName)
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

            if (index < 0) return null;

            if(args.Length == index + 1 || args[index + 1].StartsWith("-"))
            {
                return true;
            }

            if(bool.TryParse(args[index+1], out var flag))
            {
                return flag;
            }
            return null;
        }
    }
}