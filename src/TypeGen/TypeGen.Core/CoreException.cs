using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core
{
    /// <summary>
    /// An exception that occurred on TypeGen Core level
    /// </summary>
    public class CoreException : Exception
    {
        public CoreException()
        {
        }

        public CoreException(string message)
            : base(message)
        {
        }

        public CoreException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
