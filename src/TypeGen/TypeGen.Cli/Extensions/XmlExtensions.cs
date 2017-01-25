using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace TypeGen.Cli.Extensions
{
    internal static class XmlExtensions
    {
        /// <summary>
        /// Shim for .NET Framework XmlDocument.Load method
        /// </summary>
        /// <param name="document"></param>
        /// <param name="filePath"></param>
        public static void Load(this XmlDocument document, string filePath)
        {
            FileStream inStream = File.OpenRead(filePath);
            document.Load(inStream);
        }

        /// <summary>
        /// Shim for .NET Framework XmlDocument.Save method
        /// </summary>
        /// <param name="document"></param>
        /// <param name="filePath"></param>
        public static void Save(this XmlDocument document, string filePath)
        {
            FileStream outStream = File.OpenWrite(filePath);
            document.Save(outStream);
        }
    }
}
