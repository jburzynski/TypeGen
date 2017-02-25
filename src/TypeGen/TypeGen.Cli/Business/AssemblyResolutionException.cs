using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Cli.Business
{
    /// <summary>
    /// An exception that is thrown when assembly reference cannot be resolved
    /// </summary>
    internal class AssemblyResolutionException : ApplicationException
    {
        public AssemblyResolutionException()
        {
        }

        public AssemblyResolutionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public AssemblyResolutionException(string message)
            : base(message)
        {
        }

        public AssemblyResolutionException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
