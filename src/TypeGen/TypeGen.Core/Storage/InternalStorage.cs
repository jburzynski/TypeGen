using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TypeGen.Core.Storage
{
    /// <summary>
    /// Represents the internal storage
    /// </summary>
    internal class InternalStorage
    {
        /// <summary>
        /// Gets embedded resource as string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetEmbeddedResource(string name)
        {
            using (Stream stream = GetType().Assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                {
                    throw new CoreException($"Could not find embedded resource '{name}'");
                }

                var contentBytes = new byte[stream.Length];
                stream.Read(contentBytes, 0, (int)stream.Length);
                return Encoding.ASCII.GetString(contentBytes);
            }
        }
    }
}
