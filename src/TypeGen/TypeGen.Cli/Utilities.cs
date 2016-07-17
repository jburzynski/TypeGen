using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core;

namespace TypeGen.Cli
{
    internal class Utilities
    {
        /// <summary>
        /// Gets embedded resource as string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetEmbeddedResource(string name)
        {
            using (Stream stream = typeof(Utilities).Assembly.GetManifestResourceStream(name))
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

        public static string GetFileNameFromPath(string path)
        {
            return path.Split('\\').Last();
        }

        public static string ChangeFileExtension(string fileName, string toExt)
        {
            string fileNameNoExt = fileName.Split('.').First();
            return fileNameNoExt + "." + toExt;
        }
    }
}
