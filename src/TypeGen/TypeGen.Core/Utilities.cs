using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TypeGen.Core.Converters;

namespace TypeGen.Core
{
    /// <summary>
    /// Utility class
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Gets embedded resource as string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetEmbeddedResource(string name)
        {
            using (Stream stream = typeof (Utilities).Assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                {
                    throw new ApplicationException("Could not find embedded resource '" + name + "'");
                }

                var contentBytes = new byte[stream.Length];
                stream.Read(contentBytes, 0, (int)stream.Length);
                return Encoding.ASCII.GetString(contentBytes);
            }
        }

        /// <summary>
        /// Gets a string value to use as a tab
        /// </summary>
        /// <param name="tabLength">The number of spaces per tab.</param>
        /// <returns></returns>
        public static string GetTabText(int tabLength)
        {
            string tabText = string.Empty;
            for (int i = 0; i < tabLength; i++)
            {
                tabText += " ";
            }
            return tabText;
        }
    }
}
