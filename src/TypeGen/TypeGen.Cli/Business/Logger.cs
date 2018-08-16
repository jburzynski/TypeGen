using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.Validation;

namespace TypeGen.Cli.Business
{
    internal class Logger : ILogger
    {
        public void Log(params string[] messageLines)
        {
            Requires.NotNull(messageLines, nameof(messageLines));
            
            foreach (string line in messageLines)
            {
                Console.WriteLine(line);
            }
        }
    }
}
