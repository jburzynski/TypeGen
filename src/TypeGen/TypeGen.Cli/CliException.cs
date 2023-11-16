using System;
using System.Runtime.Serialization;

namespace TypeGen.Cli
{
    /// <summary>
    /// An exception that occurred on TypeGen CLI level
    /// </summary>
    internal class CliException : ApplicationException
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
