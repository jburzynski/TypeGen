using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Cli
{
    /// <summary>
    /// An exception that occurred on TypeGen CLI level
    /// </summary>
    public class CliException : ApplicationException
    {
        public CliException()
        {
        }

        public CliException(SerializationInfo info, StreamingContext context)
            : base(info, context)
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
