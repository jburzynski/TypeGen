using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Cli
{
    /// <summary>
    /// An exception that occurred on TypeGen CLI level
    /// </summary>
    internal class CliException : Exception
    {
        public CliException()
        {
        }

        public CliException(string message)
            : base(message)
        {
        }

        public CliException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
