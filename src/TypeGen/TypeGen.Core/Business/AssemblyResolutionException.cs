using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeGen.Core.Business
{
    /// <summary>
    /// An exception that is thrown when assembly reference cannot be resolved
    /// </summary>
    internal class AssemblyResolutionException : CoreException
    {
        public AssemblyResolutionException()
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
