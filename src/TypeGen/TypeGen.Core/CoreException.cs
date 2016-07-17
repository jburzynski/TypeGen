using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TypeGen.Core
{
    /// <summary>
    /// An exception that occurred on TypeGen Core level
    /// </summary>
    public class CoreException : ApplicationException
    {
        public CoreException()
        {
        }

        public CoreException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public CoreException(string message)
            : base(message)
        {
        }

        public CoreException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
