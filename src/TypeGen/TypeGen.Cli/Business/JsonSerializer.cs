using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Cli.Models;
using TypeGen.Core.Storage;
using TypeGen.Core.Validation;

namespace TypeGen.Cli.Business
{
    internal class JsonSerializer : IJsonSerializer
    {
        private readonly IFileSystem _fileSystem;

        public JsonSerializer(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public TObj DeserializeFromFile<TObj>(string filePath) where TObj : class
        {
            Requires.NotNullOrEmpty(filePath, nameof(filePath));
            
            string jsonString = _fileSystem.ReadFile(filePath);
            return Deserialize<TObj>(jsonString);
        }

        public TObj Deserialize<TObj>(string jsonString) where TObj : class
        {
            Requires.NotNullOrEmpty(jsonString, nameof(jsonString));
            
            var serializer = new DataContractJsonSerializer(typeof(TObj));
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            return (TObj)serializer.ReadObject(stream);
        }
    }
}
