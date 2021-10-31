using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core;

namespace TypeGen.IntegrationTest.Generator.TestingUtils
{
    public static class EmbededResourceReader
    {
        public static async Task<string> GetEmbeddedResourceAsync(string name)
        {
            using (Stream stream = typeof(EmbededResourceReader).GetTypeInfo().Assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                {
                    throw new CoreException($"Could not find embedded resource '{name}'");
                }

                var contentBytes = new byte[stream.Length];
                await stream.ReadAsync(contentBytes, 0, (int)stream.Length);
                var res = Encoding.UTF8.GetString(contentBytes);
                return res
                    .Trim('\uFEFF')
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Replace("\r\n", "");
            }
        }
    }
}
