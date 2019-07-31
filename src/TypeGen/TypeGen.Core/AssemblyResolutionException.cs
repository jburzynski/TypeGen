using System;

namespace TypeGen.Core
{
    /// <summary>
    /// An exception that is thrown when assembly reference cannot be resolved
    /// </summary>
    public class AssemblyResolutionException : CoreException
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
