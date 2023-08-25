using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core;
using System;

namespace TypeGen.IntegrationTest.TestingUtils
{
    public static class EmbededResourceReader
    {
        public static async Task<string> GetEmbeddedResourceAsync(string name)
        {
            await using var stream = typeof(EmbededResourceReader).GetTypeInfo().Assembly.GetManifestResourceStream(name);
            
            if (stream == null)
                throw new CoreException($"Could not find embedded resource '{name}'");

            var contentBytes = new byte[stream.Length];
            await stream.ReadAsync(contentBytes.AsMemory(0, (int)stream.Length));
            return Encoding.UTF8.GetString(contentBytes);
        }
    }
}
