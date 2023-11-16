using System.IO;
using System.Reflection;
using System.Text;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Storage
{
    /// <summary>
    /// Represents the internal storage
    /// </summary>
    internal class InternalStorage : IInternalStorage
    {
        /// <inheritdoc />
        public string GetEmbeddedResource(string name)
        {
            Requires.NotNullOrEmpty(name, nameof(name));
            
            using (Stream stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                {
                    throw new CoreException($"Could not find embedded resource '{name}'");
                }
                
                var contentBytes = new byte[stream.Length];
                stream.Read(contentBytes, 0, (int)stream.Length);
                return Encoding.UTF8.GetString(contentBytes);
            }
        }
    }
}
