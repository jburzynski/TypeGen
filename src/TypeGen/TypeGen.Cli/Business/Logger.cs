using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Cli.Business
{
    internal class Logger : ILogger
    {
        public void Log(params string[] messageLines)
        {
            foreach (string line in messageLines)
            {
                Console.WriteLine(line);
            }
        }
    }
}
