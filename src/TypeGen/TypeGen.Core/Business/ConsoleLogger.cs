using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Business
{
    /// <summary>
    /// Logs messages to the Console
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Whether to use verbose logging
        /// </summary>
        public bool LogVerbose { get; set; }
        
        /// <summary>
        /// Logs messages
        /// </summary>
        /// <param name="messageLines"></param>
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
