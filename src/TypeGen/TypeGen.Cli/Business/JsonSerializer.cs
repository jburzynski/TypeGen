using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Cli.Models;

namespace TypeGen.Cli.Business
{
    internal class JsonSerializer
    {
        private readonly FileSystem _fileSystem;

        public JsonSerializer()
        {
            _fileSystem = new FileSystem();
        }

        public TObj DeserializeFromFile<TObj>(string filePath) where TObj : class
        {
            string jsonString = _fileSystem.ReadFile(filePath);
            return Deserialize<TObj>(jsonString);
        }

        public TObj Deserialize<TObj>(string jsonString) where TObj : class
        {
            var serializer = new DataContractJsonSerializer(typeof(TgConfig));
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            return (TObj)serializer.ReadObject(stream);
        }
    }
}
