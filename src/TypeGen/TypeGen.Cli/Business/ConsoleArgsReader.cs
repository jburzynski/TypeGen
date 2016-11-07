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
        public bool ContainsHelpParam(string[] args)
        {
            return args.Any(arg => arg.ToUpperInvariant() == "-H" || arg.ToUpperInvariant() == "-HELP");
        }

        public bool ContainsGetCwdParam(string[] args)
        {
            return args.Any(arg => arg.ToUpperInvariant() == "GET-CWD");
        }

        public bool ContainsVerboseParam(string[] args)
        {
            return args.Any(arg => arg.ToUpperInvariant() == "-V" || arg.ToUpperInvariant() == "-VERBOSE");
        }

        public string GetConfigPath(string[] args)
        {
            List<string> argsList = args.ToList();
            int index = argsList.IndexOf("-Config-Path");

            if (index < 0) return null;

            if (args.Length < index + 2) // index of the next element + 1
            {
                throw new CliException("-Config-Path parameter present, but no path specified");
            }

            return args[index + 1].NormalizePath();
        }

        public string GetProjectFolder(string[] args)
        {
            return args[0].NormalizePath();
        }
    }
}
